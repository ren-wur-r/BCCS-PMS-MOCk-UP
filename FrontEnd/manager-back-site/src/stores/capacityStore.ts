import { orgMemberDirectory } from "@/constants/orgMemberDirectory";

export interface CapacityRecord {
  employeeName: string;
  departmentName: string;
  regionName: string;
  weeklyHours: number;
  weeklyManDays: number;
  monthlyHours: number;
  monthlyManDays: number;
  skills: string;
  roles: string;
  supportRegions: string[];
  unavailable: string;
  lastUpdatedAt: string | null;
}

const STORAGE_KEY = "cache.work.capacity";

const buildDefaultRecords = (): CapacityRecord[] =>
  orgMemberDirectory.map((member) => ({
    employeeName: member.name,
    departmentName: member.departmentName,
    regionName: member.regionName,
    weeklyHours: 0,
    weeklyManDays: 0,
    monthlyHours: 0,
    monthlyManDays: 0,
    skills: "",
    roles: "",
    supportRegions: member.regionName ? [member.regionName] : [],
    unavailable: "",
    lastUpdatedAt: null,
  }));

const normalizeRecord = (record: CapacityRecord): CapacityRecord => ({
  employeeName: record.employeeName,
  departmentName: record.departmentName,
  regionName: record.regionName,
  weeklyHours: Number(record.weeklyHours) || 0,
  weeklyManDays: Number(record.weeklyManDays) || 0,
  monthlyHours: Number(record.monthlyHours) || 0,
  monthlyManDays: Number(record.monthlyManDays) || 0,
  skills: record.skills ?? "",
  roles: record.roles ?? "",
  supportRegions: Array.isArray(record.supportRegions) ? record.supportRegions : [],
  unavailable: record.unavailable ?? "",
  lastUpdatedAt: record.lastUpdatedAt ?? null,
});

export const loadCapacityRecords = (): CapacityRecord[] => {
  const raw = localStorage.getItem(STORAGE_KEY);
  if (!raw) return buildDefaultRecords();
  try {
    const parsed = JSON.parse(raw);
    if (!Array.isArray(parsed)) return buildDefaultRecords();
    const normalized = parsed.map(normalizeRecord);
    const defaultRecords = buildDefaultRecords();
    const existingNames = new Set(normalized.map((item) => item.employeeName));
    defaultRecords.forEach((item) => {
      if (!existingNames.has(item.employeeName)) {
        normalized.push(item);
      }
    });
    return normalized;
  } catch {
    return buildDefaultRecords();
  }
};

export const saveCapacityRecords = (records: CapacityRecord[]) => {
  const normalized = records.map(normalizeRecord);
  localStorage.setItem(STORAGE_KEY, JSON.stringify(normalized));
};
