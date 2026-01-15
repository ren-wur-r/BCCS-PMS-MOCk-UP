import { mockDataSets } from "@/services/mockApi/mockDataSets";
import { DbAtomEmployeeProjectStatusEnum } from "@/constants/DbAtomEmployeeProjectStatusEnum";

const ASSIGNED_HOURS_PER_PROJECT = 8;
const HOURS_PER_MAN_DAY = 8;
const NEAR_CLOSE_DAYS = 30;

export const getProjectSnapshot = () => {
  const snapshotRaw = localStorage.getItem("cache.work.project.snapshot");
  if (snapshotRaw) {
    try {
      const parsed = JSON.parse(snapshotRaw);
      if (Array.isArray(parsed)) {
        return parsed;
      }
    } catch {
      return mockDataSets.workProjects;
    }
  }
  return mockDataSets.workProjects;
};

const getAssignmentSnapshot = () => {
  const raw = localStorage.getItem("cache.work.project.assignments");
  if (!raw) return [];
  try {
    const parsed = JSON.parse(raw);
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
};

export const getProjectDetailMembers = (projectId: number) => {
  const detailKey = `cache.work.project.detail.${projectId}`;
  const raw = localStorage.getItem(detailKey);
  if (!raw) return [];
  try {
    const parsed = JSON.parse(raw);
    const members = parsed?.employeeProjectMemberList;
    return Array.isArray(members) ? members : [];
  } catch {
    return [];
  }
};

export const getMemberProjects = (memberName: string) => {
  const projects = getProjectSnapshot();
  const assignmentProjects = getAssignmentSnapshot()
    .filter((item: { assigneeName?: string }) => item.assigneeName === memberName)
    .map((item: { projectId?: number; projectName?: string; companyName?: string }) => {
      const matched = projects.find((project) => project.id === item.projectId);
      if (matched) return matched;
      return {
        id: item.projectId ?? Date.now(),
        status: DbAtomEmployeeProjectStatusEnum.NotAssigned,
        name: item.projectName ?? "未命名專案",
        companyName: item.companyName ?? "",
        startTime: "",
        endTime: "",
      };
    });

  const memberProjects = projects.filter((project) => {
    const members = getProjectDetailMembers(project.id);
    return members.some((member: { employeeName?: string }) => member.employeeName === memberName);
  });

  const merged = new Map<number, (typeof projects)[number]>();
  memberProjects.forEach((project) => merged.set(project.id, project));
  assignmentProjects.forEach((project) => merged.set(project.id, project));
  return Array.from(merged.values());
};

const isInProgressStatus = (status: DbAtomEmployeeProjectStatusEnum | null | undefined) => {
  return (
    status === DbAtomEmployeeProjectStatusEnum.OnSchedule ||
    status === DbAtomEmployeeProjectStatusEnum.AtRisk ||
    status === DbAtomEmployeeProjectStatusEnum.Delayed
  );
};

const isNearClose = (endTime?: string | null) => {
  if (!endTime) return false;
  const endDate = new Date(endTime);
  if (Number.isNaN(endDate.getTime())) return false;
  const now = new Date();
  const diffMs = endDate.getTime() - now.getTime();
  const diffDays = diffMs / (1000 * 60 * 60 * 24);
  return diffDays >= 0 && diffDays <= NEAR_CLOSE_DAYS;
};

export const getMemberProjectStats = (memberName: string) => {
  const list = getMemberProjects(memberName);
  const total = list.length;
  const notStarted = list.filter(
    (item) => item.status === DbAtomEmployeeProjectStatusEnum.NotStarted
  ).length;
  const inProgress = list.filter((item) => isInProgressStatus(item.status)).length;
  const nearClose = list.filter((item) => isNearClose(item.endTime)).length;
  const assignedHours = total * ASSIGNED_HOURS_PER_PROJECT;
  const assignedManDays = assignedHours / HOURS_PER_MAN_DAY;
  return {
    total,
    notStarted,
    inProgress,
    nearClose,
    assignedHours,
    assignedManDays,
    projects: list,
  };
};

export const getMemberProjectCountMap = () => {
  const projectMapByMember = new Map<string, Set<number>>();
  const snapshot = getProjectSnapshot();
  snapshot.forEach((project) => {
    const members = getProjectDetailMembers(project.id);
    members.forEach((member: { employeeName?: string }) => {
      const name = member.employeeName || "";
      if (!name) return;
      const set = projectMapByMember.get(name) ?? new Set<number>();
      set.add(project.id);
      projectMapByMember.set(name, set);
    });
  });

  const assignments = getAssignmentSnapshot();
  assignments.forEach((item: { assigneeName?: string; projectId?: number }) => {
    const name = item.assigneeName || "";
    if (!name || !item.projectId) return;
    const set = projectMapByMember.get(name) ?? new Set<number>();
    set.add(item.projectId);
    projectMapByMember.set(name, set);
  });

  const counts = new Map<string, number>();
  projectMapByMember.forEach((set, name) => {
    counts.set(name, set.size);
  });
  return counts;
};

export const getMemberAssignedHoursMap = () => {
  const counts = getMemberProjectCountMap();
  const assigned = new Map<string, number>();
  counts.forEach((count, name) => {
    assigned.set(name, count * ASSIGNED_HOURS_PER_PROJECT);
  });
  return assigned;
};
