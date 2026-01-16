<script setup lang="ts">
import { computed, nextTick, onBeforeUnmount, onMounted, ref, watch } from "vue";
import { useRoute } from "vue-router";
import { mockAnnotations, type MockAnnotationNote } from "@/services/mockApi/mockAnnotations";

interface AnnotationNote extends MockAnnotationNote {
  source: "seed" | "local";
  scopeSelector?: string;
  offsetX?: number;
  offsetY?: number;
}

const route = useRoute();
const isAddMode = ref(false);
const storageKey = "mock-annotations";
const hiddenStorageKey = "mock-annotations-hidden";
const localNotes = ref<AnnotationNote[]>([]);
const runtimeNotes = ref<AnnotationNote[]>([]);
const hiddenSeedIds = ref<string[]>([]);
const expandedStorageKey = "mock-annotations-expanded";
const expandedNoteIds = ref<string[]>([]);
const sizeExpandedStorageKey = "mock-annotations-size-expanded";
const sizeExpandedNoteIds = ref<string[]>([]);

const isPathMatch = (notePath: string, currentPaths: string[]) => {
  const candidates = currentPaths.filter(Boolean);
  if (notePath.endsWith("*")) {
    const prefix = notePath.slice(0, -1);
    return candidates.some((currentPath) => currentPath.startsWith(prefix));
  }
  return candidates.includes(notePath);
};

const loadLocalNotes = () => {
  const raw = localStorage.getItem(storageKey);
  if (!raw) {
    localNotes.value = [];
    return;
  }
  try {
    const parsed = JSON.parse(raw) as AnnotationNote[];
    localNotes.value = Array.isArray(parsed) ? parsed : [];
  } catch {
    localNotes.value = [];
  }
};

const loadHiddenSeeds = () => {
  const raw = localStorage.getItem(hiddenStorageKey);
  if (!raw) {
    hiddenSeedIds.value = [];
    return;
  }
  try {
    const parsed = JSON.parse(raw) as string[];
    hiddenSeedIds.value = Array.isArray(parsed) ? parsed : [];
  } catch {
    hiddenSeedIds.value = [];
  }
};

const persistHiddenSeeds = () => {
  localStorage.setItem(hiddenStorageKey, JSON.stringify(hiddenSeedIds.value));
};

const loadExpandedNotes = () => {
  const raw = localStorage.getItem(expandedStorageKey);
  if (!raw) {
    expandedNoteIds.value = [];
    return;
  }
  try {
    const parsed = JSON.parse(raw) as string[];
    expandedNoteIds.value = Array.isArray(parsed) ? parsed : [];
  } catch {
    expandedNoteIds.value = [];
  }
};

const persistExpandedNotes = () => {
  localStorage.setItem(expandedStorageKey, JSON.stringify(expandedNoteIds.value));
};

const loadSizeExpandedNotes = () => {
  const raw = localStorage.getItem(sizeExpandedStorageKey);
  if (!raw) {
    sizeExpandedNoteIds.value = [];
    return;
  }
  try {
    const parsed = JSON.parse(raw) as string[];
    sizeExpandedNoteIds.value = Array.isArray(parsed) ? parsed : [];
  } catch {
    sizeExpandedNoteIds.value = [];
  }
};

const persistSizeExpandedNotes = () => {
  localStorage.setItem(
    sizeExpandedStorageKey,
    JSON.stringify(sizeExpandedNoteIds.value)
  );
};

const persistLocalNotes = () => {
  localStorage.setItem(storageKey, JSON.stringify(localNotes.value));
};

const createSelector = (el: Element, root?: Element): string => {
  const element = el as HTMLElement;
  const scopeValue = element.getAttribute("data-annotation-scope");
  if (scopeValue) return `[data-annotation-scope="${scopeValue}"]`;
  if (!root && element.id) return `#${element.id}`;
  const path: string[] = [];
  let current: Element | null = element;
  const stopAt = root ?? document.body;
  while (current && current.nodeType === Node.ELEMENT_NODE && current !== stopAt) {
    const tag = current.tagName.toLowerCase();
    const parent = current.parentElement;
    if (!parent) break;
    const siblings = Array.from(parent.children).filter(
      (child) => child.tagName.toLowerCase() === tag
    );
    const index = siblings.indexOf(current) + 1;
    path.unshift(`${tag}:nth-of-type(${index})`);
    current = parent;
  }
  if (current === stopAt) {
    return path.length > 0 ? path.join(" > ") : ":scope";
  }
  return path.length > 0 ? path.join(" > ") : "body";
};

const clampPosition = (x: number, y: number) => {
  const padding = 12;
  const maxX = window.innerWidth - padding;
  const maxY = window.innerHeight - padding;
  return {
    x: Math.min(Math.max(x, padding), maxX),
    y: Math.min(Math.max(y, padding), maxY),
  };
};

const resolveNotePosition = (note: AnnotationNote) => {
  if (note.selector) {
    const scopeElement = note.scopeSelector
      ? (document.querySelector(note.scopeSelector) as Element | null)
      : null;
    if (note.scopeSelector && !scopeElement) {
      return null;
    }
    const element =
      note.selector === ":scope"
        ? scopeElement
        : scopeElement
          ? scopeElement.querySelector(note.selector)
          : document.querySelector(note.selector);
    if (!element) {
      return null;
    }
    const isElementHidden = (el: Element) => {
      let current: Element | null = el;
      while (current) {
        if (
          current.hasAttribute("hidden") ||
          current.getAttribute("aria-hidden") === "true"
        ) {
          return true;
        }
        const style = window.getComputedStyle(current);
        if (
          style.display === "none" ||
          style.visibility === "hidden" ||
          style.opacity === "0"
        ) {
          return true;
        }
        current = current.parentElement;
      }
      return false;
    };
    const rect = element.getBoundingClientRect();
    const hasLayout = element.getClientRects().length > 0;
    if (!hasLayout || rect.width === 0 || rect.height === 0 || isElementHidden(element)) {
      return null;
    }
    const isVisible =
      rect.bottom >= 0 &&
      rect.right >= 0 &&
      rect.top <= window.innerHeight &&
      rect.left <= window.innerWidth;
    if (!isVisible) {
      return null;
    }
    const anchoredX =
      typeof note.offsetX === "number" ? rect.left + note.offsetX : rect.left;
    const anchoredY =
      typeof note.offsetY === "number" ? rect.top + note.offsetY : rect.top;
    const pos = clampPosition(anchoredX, anchoredY);
    return { ...note, x: pos.x, y: pos.y };
  }
  if (typeof note.x === "number" && typeof note.y === "number") {
    return note;
  }
  return null;
};

const rebuildRuntimeNotes = () => {
  const currentFullPath = route.fullPath;
  const currentPaths = [route.path, route.fullPath];
  const seed = mockAnnotations
    .filter((note) => isPathMatch(note.routePath, currentPaths))
    .filter((note) => !hiddenSeedIds.value.includes(note.id))
    .map((note) => ({ ...note, source: "seed" as const }));
  const locals = localNotes.value.filter((note) => note.routePath === currentFullPath);
  const merged = new Map<string, AnnotationNote>();
  seed.forEach((note) => merged.set(note.id, note));
  locals.forEach((note) => merged.set(note.id, note));
  runtimeNotes.value = Array.from(merged.values())
    .map(resolveNotePosition)
    .filter(Boolean) as AnnotationNote[];
};

let rebuildFrame = 0;
const scheduleRebuild = () => {
  if (rebuildFrame) return;
  rebuildFrame = window.requestAnimationFrame(() => {
    rebuildFrame = 0;
    rebuildRuntimeNotes();
  });
};

const updateLocalNote = (note: AnnotationNote) => {
  const idx = localNotes.value.findIndex((item) => item.id === note.id);
  if (idx >= 0) {
    localNotes.value[idx] = { ...note, source: "local" };
  } else {
    localNotes.value.push({ ...note, source: "local" });
  }
  persistLocalNotes();
  rebuildRuntimeNotes();
};

const removeNote = (note: AnnotationNote) => {
  if (note.source === "seed") {
    if (!hiddenSeedIds.value.includes(note.id)) {
      hiddenSeedIds.value.push(note.id);
      persistHiddenSeeds();
    }
    expandedNoteIds.value = expandedNoteIds.value.filter((id) => id !== note.id);
    persistExpandedNotes();
    rebuildRuntimeNotes();
    return;
  }
  localNotes.value = localNotes.value.filter((item) => item.id !== note.id);
  persistLocalNotes();
  expandedNoteIds.value = expandedNoteIds.value.filter((id) => id !== note.id);
  persistExpandedNotes();
  sizeExpandedNoteIds.value = sizeExpandedNoteIds.value.filter((id) => id !== note.id);
  persistSizeExpandedNotes();
  rebuildRuntimeNotes();
};

const handleAddClick = (event: MouseEvent) => {
  if (!isAddMode.value) return;
  const target = event.target as HTMLElement | null;
  if (!target || target.closest(".annotation-overlay")) return;
  event.preventDefault();
  event.stopPropagation();
  const scopeRoot = target.closest("[data-annotation-scope], .tab") as HTMLElement | null;
  const scopeSelector = scopeRoot ? createSelector(scopeRoot) : undefined;
  const selector = scopeRoot ? createSelector(target, scopeRoot) : createSelector(target);
  const rect = target.getBoundingClientRect();
  const offsetX = event.clientX - rect.left;
  const offsetY = event.clientY - rect.top;
  const pos = clampPosition(rect.left, rect.top);
  const note: AnnotationNote = {
    id: `local-${Date.now()}`,
    routePath: route.fullPath,
    selector,
    scopeSelector,
    x: pos.x,
    y: pos.y,
    offsetX,
    offsetY,
    text: "",
    source: "local",
  };
  localNotes.value.push(note);
  persistLocalNotes();
  expandedNoteIds.value = [...new Set([...expandedNoteIds.value, note.id])];
  persistExpandedNotes();
  rebuildRuntimeNotes();
  isAddMode.value = false;
  nextTick(() => {
    const textarea = document.querySelector(
      `.annotation-note[data-note-id="${note.id}"] textarea`
    ) as HTMLTextAreaElement | null;
    textarea?.focus();
  });
};

const toggleAddMode = () => {
  isAddMode.value = !isAddMode.value;
};

const updateNoteText = (note: AnnotationNote, value: string) => {
  updateLocalNote({ ...note, text: value });
  if (!value.trim()) {
    if (!expandedNoteIds.value.includes(note.id)) {
      expandedNoteIds.value.push(note.id);
      persistExpandedNotes();
    }
  }
};

const isNoteExpanded = (note: AnnotationNote) =>
  !note.text?.trim() || expandedNoteIds.value.includes(note.id);

const isNoteSizeExpanded = (note: AnnotationNote) =>
  sizeExpandedNoteIds.value.includes(note.id);

const collapseNote = (note: AnnotationNote) => {
  if (!note.text?.trim()) return;
  expandedNoteIds.value = expandedNoteIds.value.filter((id) => id !== note.id);
  persistExpandedNotes();
};

const expandNote = (note: AnnotationNote) => {
  if (!expandedNoteIds.value.includes(note.id)) {
    expandedNoteIds.value.push(note.id);
    persistExpandedNotes();
  }
  nextTick(() => {
    const textarea = document.querySelector(
      `.annotation-note[data-note-id="${note.id}"] textarea`
    ) as HTMLTextAreaElement | null;
    textarea?.focus();
  });
};

const toggleNoteSize = (note: AnnotationNote) => {
  if (sizeExpandedNoteIds.value.includes(note.id)) {
    sizeExpandedNoteIds.value = sizeExpandedNoteIds.value.filter((id) => id !== note.id);
  } else {
    sizeExpandedNoteIds.value = [...sizeExpandedNoteIds.value, note.id];
  }
  persistSizeExpandedNotes();
};

const collapseAllExpanded = (event: MouseEvent) => {
  if (isAddMode.value) return;
  const target = event.target as HTMLElement | null;
  if (!target) return;
  if (target.closest(".annotation-note")) return;
  if (target.closest(".annotation-panel")) return;
  if (target.closest(".annotation-bubble")) return;
  const nextExpanded = expandedNoteIds.value.filter((id) => {
    const note = runtimeNotes.value.find((item) => item.id === id);
    return !note?.text?.trim();
  });
  if (nextExpanded.length !== expandedNoteIds.value.length) {
    expandedNoteIds.value = nextExpanded;
    persistExpandedNotes();
  }
};

let mutationObserver: MutationObserver | null = null;

onMounted(() => {
  loadLocalNotes();
  loadHiddenSeeds();
  loadExpandedNotes();
  loadSizeExpandedNotes();
  rebuildRuntimeNotes();
  nextTick(() => rebuildRuntimeNotes());
  document.addEventListener("click", handleAddClick, true);
  document.addEventListener("click", collapseAllExpanded);
  window.addEventListener("scroll", scheduleRebuild, true);
  window.addEventListener("resize", scheduleRebuild);

  mutationObserver = new MutationObserver(() => {
    scheduleRebuild();
  });
  mutationObserver.observe(document.body, {
    childList: true,
    subtree: true,
    attributes: true,
    attributeFilter: ['data-annotation-scope', 'hidden', 'aria-hidden', 'style'],
  });
});

onBeforeUnmount(() => {
  document.removeEventListener("click", handleAddClick, true);
  document.removeEventListener("click", collapseAllExpanded);
  window.removeEventListener("scroll", scheduleRebuild, true);
  window.removeEventListener("resize", scheduleRebuild);
  if (mutationObserver) {
    mutationObserver.disconnect();
    mutationObserver = null;
  }
});

watch(
  () => route.fullPath,
  () => {
    rebuildRuntimeNotes();
    nextTick(() => rebuildRuntimeNotes());
  }
);
</script>

<template>
  <teleport to="body">
    <div class="annotation-overlay">
      <div class="annotation-panel">
        <button
          class="annotation-panel__button"
          :class="{ active: isAddMode }"
          type="button"
          @click="toggleAddMode"
        >
          新增註解
        </button>
        <div v-if="isAddMode" class="annotation-panel__hint">點選畫面任一區塊放置說明</div>
      </div>

      <div v-for="note in runtimeNotes" :key="note.id" :data-note-id="note.id">
        <div
          v-if="isNoteExpanded(note)"
          class="annotation-note"
          :class="{ 'annotation-note--expanded': isNoteSizeExpanded(note) }"
          :style="{ left: `${note.x ?? 24}px`, top: `${note.y ?? 120}px` }"
        >
          <div class="annotation-note__header">
            <span class="annotation-note__title">說明</span>
            <div class="annotation-note__actions">
              <button
                class="annotation-note__toggle"
                type="button"
                @click="toggleNoteSize(note)"
              >
                {{ isNoteSizeExpanded(note) ? "收合" : "展開" }}
              </button>
              <button class="annotation-note__delete" type="button" @click="removeNote(note)">
                刪除
              </button>
            </div>
          </div>
          <textarea
            class="annotation-note__textarea"
            :value="note.text"
            rows="3"
            placeholder="輸入說明..."
            @input="updateNoteText(note, ($event.target as HTMLTextAreaElement).value)"
            @blur="collapseNote(note)"
          ></textarea>
        </div>
        <button
          v-else
          class="annotation-bubble"
          type="button"
          :title="note.text"
          :style="{ left: `${note.x ?? 24}px`, top: `${note.y ?? 120}px` }"
          @click="expandNote(note)"
        >
          <svg viewBox="0 0 24 24" aria-hidden="true">
            <path
              d="M4 5.5a3.5 3.5 0 0 1 3.5-3.5h9A3.5 3.5 0 0 1 20 5.5v7A3.5 3.5 0 0 1 16.5 16H9l-3.6 3a.75.75 0 0 1-1.23-.57V16A3.5 3.5 0 0 1 4 12.5v-7Z"
              fill="currentColor"
            />
          </svg>
        </button>
      </div>
    </div>
  </teleport>
</template>

<style scoped>
.annotation-overlay {
  position: fixed;
  inset: 0;
  pointer-events: none;
  z-index: 80;
}

.annotation-panel {
  position: fixed;
  right: 20px;
  bottom: 20px;
  display: flex;
  flex-direction: column;
  gap: 6px;
  pointer-events: auto;
}

.annotation-panel__button {
  border-radius: 999px;
  border: 1px solid #7c3aed;
  background: #7c3aed;
  color: #ffffff;
  padding: 8px 14px;
  font-size: 12px;
  cursor: pointer;
}

.annotation-panel__button.active {
  background: #5b21b6;
  border-color: #5b21b6;
}

.annotation-panel__hint {
  background: #ffffff;
  border: 1px solid #e5e7eb;
  color: #475569;
  padding: 6px 10px;
  border-radius: 8px;
  font-size: 11px;
  box-shadow: 0 4px 12px rgba(15, 23, 42, 0.12);
}

.annotation-note {
  position: fixed;
  width: 240px;
  background: #ffffff;
  border: 1px solid #e2e8f0;
  border-radius: 10px;
  padding: 10px;
  box-shadow: 0 8px 20px rgba(15, 23, 42, 0.14);
  pointer-events: auto;
}

.annotation-note--expanded {
  width: 380px;
  max-width: 90vw;
}

.annotation-bubble {
  position: fixed;
  width: 38px;
  height: 38px;
  border-radius: 999px;
  border: 1px solid #cbd5f5;
  background: #7c3aed;
  color: #ffffff;
  box-shadow: 0 8px 18px rgba(124, 58, 237, 0.28);
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  pointer-events: auto;
}

.annotation-bubble svg {
  width: 18px;
  height: 18px;
}

.annotation-note__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 6px;
}

.annotation-note__actions {
  display: inline-flex;
  align-items: center;
  gap: 8px;
}

.annotation-note__title {
  font-size: 12px;
  font-weight: 600;
  color: #0f172a;
}

.annotation-note__toggle {
  border: none;
  background: transparent;
  color: #0ea5e9;
  font-size: 11px;
  cursor: pointer;
  padding: 0;
}

.annotation-note__delete {
  border: none;
  background: transparent;
  color: #ef4444;
  font-size: 11px;
  cursor: pointer;
  padding: 0;
}

.annotation-note__textarea {
  width: 100%;
  min-height: 70px;
  border: 1px solid #cbd5f5;
  border-radius: 8px;
  padding: 6px;
  font-size: 12px;
  resize: both;
}

.annotation-note--expanded .annotation-note__textarea {
  min-height: 160px;
}
</style>
