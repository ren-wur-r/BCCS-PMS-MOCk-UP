import type { AxiosInstance, AxiosRequestConfig, AxiosResponse } from "axios";
import { DbAtomCityEnum } from "@/constants/DbAtomCityEnum";
import { orgMemberDirectory } from "@/constants/orgMemberDirectory";
import { orgDepartmentDirectory } from "@/constants/orgDepartmentDirectory";
import { mockDataSets, getPaginatedData, filterData } from "./mockDataSets";

interface MockResponse<T = any> {
  data: T;
  status: number;
  statusText: string;
  headers: any;
  config: AxiosRequestConfig;
}

export class MockApiClient {
  public baseURL: string;
  public headers: any;
  private cachePrefix = "cache.api";

  constructor(config: { baseURL?: string; headers?: any }) {
    this.baseURL = config.baseURL || "";
    this.headers = config.headers || {};
  }

  interceptors = {
    request: {
      use: (successHandler: any, errorHandler?: any) => {
        return 0;
      },
    },
    response: {
      use: (successHandler: any, errorHandler?: any) => {
        return 0;
      },
    },
  };

  private safeStringify(value: unknown) {
    try {
      if (typeof value === "string") {
        try {
          return JSON.stringify(JSON.parse(value));
        } catch {
          return JSON.stringify({ raw: value });
        }
      }
      return JSON.stringify(value ?? {});
    } catch {
      return "{}";
    }
  }

  private readCache<T>(method: string, endpoint: string, requestData?: any): T | null {
    const normalizedEndpoint = endpoint.startsWith("/") ? endpoint.slice(1) : endpoint;
    const key = `${this.cachePrefix}:${method}:${normalizedEndpoint}:${this.safeStringify(
      requestData ?? {}
    )}`;
    const raw = localStorage.getItem(key);
    if (!raw) return null;
    try {
      return JSON.parse(raw) as T;
    } catch {
      return null;
    }
  }

  private readWorkProjectSnapshot() {
    const raw = localStorage.getItem("cache.work.project.snapshot");
    if (!raw) return null;
    try {
      const parsed = JSON.parse(raw);
      if (!Array.isArray(parsed) || parsed.length === 0) return null;
      return parsed;
    } catch {
      return null;
    }
  }

  private readSystemProductSnapshot<T>(key: string) {
    const raw = localStorage.getItem(key);
    if (!raw) return null;
    try {
      const parsed = JSON.parse(raw);
      if (!Array.isArray(parsed) || parsed.length === 0) return null;
      return parsed as T[];
    } catch {
      return null;
    }
  }

  private async mockRequest<T>(
    method: string,
    endpoint: string,
    data?: any,
    delay: number = 500
  ): Promise<MockResponse<T>> {
    await new Promise((resolve) => setTimeout(resolve, delay));

    const mockData = this.getMockDataForEndpoint<T>(method, endpoint, data);

    return {
      data: mockData,
      status: 200,
      statusText: "OK",
      headers: {},
      config: {},
    };
  }

  async get<T>(url: string, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
    return this.mockRequest<T>("get", url) as Promise<AxiosResponse<T>>;
  }

  async post<T>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig
  ): Promise<AxiosResponse<T>> {
    return this.mockRequest<T>("post", url, data) as Promise<AxiosResponse<T>>;
  }

  async put<T>(
    url: string,
    data?: any,
    config?: AxiosRequestConfig
  ): Promise<AxiosResponse<T>> {
    return this.mockRequest<T>("put", url, data) as Promise<AxiosResponse<T>>;
  }

  async delete<T>(url: string, config?: AxiosRequestConfig): Promise<AxiosResponse<T>> {
    return this.mockRequest<T>("delete", url) as Promise<AxiosResponse<T>>;
  }

  private getMockDataForEndpoint<T>(method: string, endpoint: string, requestData?: any): T {
    const normalizedEndpoint = endpoint.startsWith("/") ? endpoint.slice(1) : endpoint;
    const cached = this.readCache<T>(method, endpoint, requestData);
    if (cached && normalizedEndpoint !== "api/MbsWorkProject/GetManyProject") {
      return cached;
    }

    let requestPageIndex = 1;
    let requestPageSize = 10;

    if (typeof requestData?.i === "number") {
      requestPageIndex = requestData?.h ?? 1;
      requestPageSize = requestData?.i ?? 10;
    } else if (
      typeof requestData?.g === "number" ||
      typeof requestData?.h === "number"
    ) {
      requestPageIndex = requestData?.g ?? 1;
      requestPageSize = requestData?.h ?? 10;
    } else if (
      typeof requestData?.c === "number" ||
      typeof requestData?.d === "number"
    ) {
      requestPageIndex = requestData?.c ?? 1;
      requestPageSize = requestData?.d ?? 10;
    } else {
      requestPageIndex = requestData?.e ?? requestData?.PageIndex ?? 1;
      requestPageSize = requestData?.f ?? requestData?.PageSize ?? 10;
    }

    const pageIndex = requestPageIndex > 0 ? requestPageIndex - 1 : 0;
    const pageSize = requestPageSize > 0 ? requestPageSize : 10;

    const mockDataMap: { [key: string]: () => any } = {
      "api/MbsBasic/Login": () => ({
        aa: 1,
        a: "mock-token-" + Date.now(),
      }),

      "api/MbsBasic/Logout": () => ({
        aa: 1,
      }),

      "api/MbsBasic/Heartbeat": () => ({
        aa: 1,
      }),

      "api/MbsSystemEmployee/GetMany": () => {
        const { items, totalCount } = getPaginatedData(
          mockDataSets.employees,
          pageIndex,
          pageSize
        );
        return {
          aa: 0,
          a: items,
          b: totalCount,
        };
      },

      "api/MbsSystemCompany/GetMany": () => {
        const { items, totalCount } = getPaginatedData(
          mockDataSets.companies,
          pageIndex,
          pageSize
        );
        return {
          aa: 0,
          a: items,
          b: totalCount,
        };
      },

      "api/MbsBasic/GetManyManagerCompany": () => {
        const keyword = requestData?.a ? String(requestData.a).toLowerCase() : "";
        const filtered = keyword
          ? mockDataSets.companies.filter((item) =>
              String(item.CompanyName).toLowerCase().includes(keyword)
            )
          : mockDataSets.companies;
        const { items } = getPaginatedData(filtered, pageIndex, pageSize);
        return {
          aa: 0,
          a: items.map((item) => ({
            a: Number(String(item.CompanyID).replace(/\D/g, "")) || 1,
            b: item.CompanyName,
          })),
        };
      },

      "api/MbsBasic/GetManyEmployeeInfo": () => {
        const keyword = requestData?.f ? String(requestData.f).toLowerCase() : "";
        const baseList =
          orgMemberDirectory.length > 0
            ? orgMemberDirectory
            : mockDataSets.employees.map((item) => ({
                name: item.EmployeeName,
                email: item.EmployeeEmail ?? "",
                regionName: "北區",
                departmentName: item.DepartmentName ?? "工程部",
              }));
        let filtered = keyword
          ? baseList.filter((item) => item.name.toLowerCase().includes(keyword))
          : baseList;
        const regionId = Number(requestData?.d) || 0;
        const departmentId = Number(requestData?.e) || 0;
        const regionName =
          regionId === 2 ? "中區" : regionId === 3 ? "南區" : regionId === 1 ? "北區" : "";
        if (regionName && regionName !== "跨區") {
          filtered = filtered.filter((item) => item.regionName === regionName);
        }
        const departmentMap = new Map();
        let deptIdCounter = 1;
        const sourceDepartments =
          orgDepartmentDirectory.length > 0 ? orgDepartmentDirectory : baseList.map((i) => i.departmentName);
        for (const name of sourceDepartments) {
          if (!departmentMap.has(name)) {
            departmentMap.set(name, deptIdCounter);
            deptIdCounter += 1;
          }
        }
        if (departmentId) {
          filtered = filtered.filter(
            (item) => departmentMap.get(item.departmentName) === departmentId
          );
        }
        const { items } = getPaginatedData(filtered, pageIndex, pageSize);
        return {
          aa: 0,
          a: items.map((item, index) => ({
            a: index + 1,
            b: item.name,
            c: true,
            d:
              item.regionName === "中區"
                ? 2
                : item.regionName === "南區"
                  ? 3
                  : item.regionName === "北區"
                    ? 1
                    : regionId || 1,
            e: item.regionName,
            f: departmentMap.get(item.departmentName) ?? 1,
            g: item.departmentName,
          })),
        };
      },

      "api/MbsSystemCompany/GetManyCompanyLocation": () => {
        const companyId = requestData?.a;
        const fallbackCity =
          companyId === 1 ? DbAtomCityEnum.Taipei : DbAtomCityEnum.Taichung;
        return {
          aa: 0,
          a: [
            {
              a: 1,
              b: "主要營業地點",
              c: fallbackCity,
              d: "台灣",
              e: "02-1234-5678",
              f: 1,
            },
          ],
        };
      },

      "api/MbsBasic/GetAllManagerDepartment": () => ({
        aa: 0,
        a: orgDepartmentDirectory.map((name, index) => ({
          a: index + 1,
          b: name,
        })),
      }),

      "api/MbsCrmPipeline/GetManyEmployeePipeline": () => {
        const stored = localStorage.getItem("cache.crm.pipeline.list");
        const pipelineSource = stored ? JSON.parse(stored) : mockDataSets.crmPipelines;
        const { items, totalCount } = getPaginatedData(pipelineSource, pageIndex, pageSize);
        return {
          aa: 1,
          a: items.map((item) => ({
            a: item.id,
            b: item.status,
            c: item.companyName,
            d: item.contacterDepartment,
            e: item.contacterJobTitle,
            f: item.contacterName,
            g: item.contacterEmail,
            h: item.contacterPhone,
            i: item.contacterTelephone,
            j: item.salerEmployeeName,
          })),
          b: totalCount,
        };
      },
      "api/MbsSystemProduct/GetMany": () => {
        const productSnapshot =
          this.readSystemProductSnapshot<any>("cache.system.product.list") ?? [];
        const mainKindSnapshot =
          this.readSystemProductSnapshot<any>("cache.system.product.main-kind.list") ?? [];
        const subKindSnapshot =
          this.readSystemProductSnapshot<any>("cache.system.product.sub-kind.list") ?? [];
        const mainKindId = requestData?.a ?? null;
        const subKindId = requestData?.b ?? null;
        const isKey = requestData?.c ?? null;
        const nameQuery = (requestData?.d ?? "").toString().trim().toLowerCase();
        const specQuery = (requestData?.e ?? "").toString().trim().toLowerCase();
        const specEnable = requestData?.f ?? null;

        const mainKindName =
          mainKindId == null
            ? null
            : mainKindSnapshot.find((item: any) => item.managerProductMainKindID === mainKindId)
                ?.managerProductMainKindName ?? null;
        const subKindName =
          subKindId == null
            ? null
            : subKindSnapshot.find((item: any) => item.managerProductSubKindID === subKindId)
                ?.managerProductSubKindName ?? null;

        const filtered = productSnapshot.filter((item: any) => {
          if (mainKindName && item.managerProductMainKindName !== mainKindName) return false;
          if (subKindName && item.managerProductSubKindName !== subKindName) return false;
          if (isKey !== null && item.managerProductIsKey !== isKey) return false;
          if (specEnable !== null && item.managerProductSpecificationIsEnable !== specEnable) {
            return false;
          }
          if (nameQuery && !String(item.managerProductName ?? "").toLowerCase().includes(nameQuery)) {
            return false;
          }
          if (
            specQuery &&
            !String(item.managerProductSpecificationName ?? "").toLowerCase().includes(specQuery)
          ) {
            return false;
          }
          return true;
        });

        const { items, totalCount } = getPaginatedData(filtered, pageIndex, pageSize);
        return {
          aa: 1,
          a: items.map((item: any) => ({
            a: item.managerProductID,
            b: item.managerProductName,
            c: item.managerProductMainKindName,
            d: item.managerProductSubKindName,
            e: item.managerProductIsKey,
            f: item.managerProductSpecificationName,
            g: item.managerProductSpecificationSellPrice,
            h: item.managerProductSpecificationCostPrice,
            i: item.managerProductSpecificationIsEnable,
          })),
          b: totalCount,
        };
      },
      "api/MbsSystemProduct/GetManyMainKind": () => {
        const mainKindSnapshot =
          this.readSystemProductSnapshot<any>("cache.system.product.main-kind.list") ?? [];
        const nameQuery = (requestData?.a ?? "").toString().trim().toLowerCase();
        const isEnable = requestData?.b ?? null;
        const filtered = mainKindSnapshot.filter((item: any) => {
          if (isEnable !== null && item.managerProductMainKindIsEnable !== isEnable) {
            return false;
          }
          if (
            nameQuery &&
            !String(item.managerProductMainKindName ?? "").toLowerCase().includes(nameQuery)
          ) {
            return false;
          }
          return true;
        });
        const localPageIndex =
          typeof requestData?.c === "number" ? Math.max(0, requestData.c - 1) : pageIndex;
        const localPageSize = typeof requestData?.d === "number" ? requestData.d : pageSize;
        const { items, totalCount } = getPaginatedData(filtered, localPageIndex, localPageSize);
        return {
          aa: 1,
          a: items.map((item: any) => ({
            a: item.managerProductMainKindID,
            b: item.managerProductMainKindName,
            c: item.managerProductMainKindCommissionRate,
            d: item.managerProductMainKindIsEnable,
          })),
          b: totalCount,
        };
      },
      "api/MbsSystemProduct/GetManySubKind": () => {
        const subKindSnapshot =
          this.readSystemProductSnapshot<any>("cache.system.product.sub-kind.list") ?? [];
        const mainKindId = requestData?.a ?? null;
        const nameQuery = (requestData?.b ?? "").toString().trim().toLowerCase();
        const isEnable = requestData?.c ?? null;
        const filtered = subKindSnapshot.filter((item: any) => {
          if (mainKindId !== null && item.managerProductMainKindID !== mainKindId) return false;
          if (isEnable !== null && item.managerProductSubKindIsEnable !== isEnable) return false;
          if (
            nameQuery &&
            !String(item.managerProductSubKindName ?? "").toLowerCase().includes(nameQuery)
          ) {
            return false;
          }
          return true;
        });
        const localPageIndex =
          typeof requestData?.d === "number" ? Math.max(0, requestData.d - 1) : pageIndex;
        const localPageSize = typeof requestData?.e === "number" ? requestData.e : pageSize;
        const { items, totalCount } = getPaginatedData(filtered, localPageIndex, localPageSize);
        return {
          aa: 1,
          a: items.map((item: any) => ({
            a: item.managerProductMainKindID,
            b: item.managerProductMainKindName,
            c: item.managerProductSubKindID,
            d: item.managerProductSubKindName,
            e: item.managerProductSubKindCommissionRate,
            f: item.managerProductSubKindIsEnable,
          })),
          b: totalCount,
        };
      },

      "api/MbsCrmPipeline/GetEmployeePipeline": () => {
        const pipelineId = requestData?.a;
        const detailKey = `cache.crm.pipeline.detail.${pipelineId}`;
        const detailRaw = localStorage.getItem(detailKey);
        const detail =
          (detailRaw ? JSON.parse(detailRaw) : null) ??
          mockDataSets.crmPipelines.find((item) => item.id === pipelineId) ??
          mockDataSets.crmPipelines[0];
        const regionCode = detail?.customerRegionCode ?? "";
        const fallbackCity =
          regionCode === "A"
            ? DbAtomCityEnum.Taipei
            : regionCode === "B"
              ? DbAtomCityEnum.Taichung
              : regionCode === "C"
                ? DbAtomCityEnum.Kaohsiung
                : null;

        return {
          aa: 1,
          a: detail?.status ?? 2,
          b: 1001,
          c: detail?.salerEmployeeName ?? "王小明",
          d: 1,
          e: "北區",
          f: 1,
          g: "業務部",
          h: {
            a: detail?.managerCompanyUnifiedNumber ?? "12345678",
            b: detail?.id ?? 1,
            c: detail?.companyName ?? "範例公司",
            d: 1,
            e: 1,
            f: "一般企業",
            g: "科技業",
            h: detail?.atomCity ?? fallbackCity ?? 1,
            i: 1,
            j: "台北市信義區松仁路100號",
            k: "02-2345-6789",
            l: 1,
          },
          i: [
            {
              a: 1,
              b: true,
              c: detail?.contacterName ?? "王小明",
              d: detail?.contacterEmail ?? "contact@example.com",
              e: detail?.contacterPhone ?? "0912-345-678",
              f: detail?.contacterDepartment ?? "資訊部",
              g: detail?.contacterJobTitle ?? "經理",
              h: detail?.contacterTelephone ?? "02-1234-5678",
              i: true,
              j: 1,
              k: 1,
              l: "主要聯絡人",
            },
          ],
          j: null,
          k: [],
          l: [],
          m: [],
          n: [],
          o: [],
          p: null,
        };
      },

      "api/MbsWorkProject/GetMany": () => {
        const { items, totalCount } = getPaginatedData(
          mockDataSets.projects,
          pageIndex,
          pageSize
        );
        return {
          aa: 0,
          a: items,
          b: totalCount,
        };
      },
      "api/MbsWorkProject/GetManyProject": () => {
        const workProjectSnapshot = this.readWorkProjectSnapshot();
        const workProjectSource = workProjectSnapshot ?? mockDataSets.workProjects;
        const { items, totalCount } = getPaginatedData(
          workProjectSource,
          pageIndex,
          pageSize
        );
        return {
          aa: 1,
          a: items.map((item) => ({
            a: item.id,
            b: item.status,
            c: item.name,
            d: item.companyName,
            e: item.startTime,
            f: item.endTime,
          })),
          b: totalCount,
        };
      },
      "api/MbsWorkProject/GetProject": () => {
        const projectId = requestData?.a;
        const detailCacheKey = `cache.work.project.detail.${projectId}`;
        const detailRaw = localStorage.getItem(detailCacheKey);
        if (detailRaw) {
          try {
            const detail = JSON.parse(detailRaw);
            return {
              aa: detail.errorCode,
              a: detail.atomEmployeeProjectStatus,
              b: detail.employeeProjectCode,
              c: detail.employeeProjectContractCreateTime,
              d: detail.employeeProjectContractUrl,
              e: detail.employeeProjectWorkCreateTime,
              f: detail.employeeProjectWorkUrl,
              g:
                detail.employeeProjectMemberList?.map((item: any) => ({
                  a: item.employeeProjectMemberID,
                  b: item.employeeProjectMemberRole,
                  c: item.managerRegionName,
                  d: item.managerDepartmentName,
                  e: item.employeeName,
                  f: item.employeeID,
                })) ?? [],
              h: detail.employeeProjectName,
              i: detail.employeeProjectStartTime,
              j: detail.employeeProjectEndTime,
              k: detail.managerCompanyName,
            };
          } catch {
            // fall back to snapshot
          }
        }
        const workProjectSnapshot = this.readWorkProjectSnapshot();
        const workProjectSource = workProjectSnapshot ?? mockDataSets.workProjects;
        const project =
          workProjectSource.find((item: any) => item.id === projectId) ??
          workProjectSource[0];

        return {
          aa: 1,
          a: project?.status ?? 2,
          b: `PRJ-${String(project?.id ?? 1).padStart(4, "0")}`,
          c: "",
          d: "",
          e: "",
          f: "",
          g: [
            {
              a: 3,
              b: 3,
              c: "北區",
              d: "專案管理部",
              e: "林志偉",
              f: 2003,
            },
          ],
          h: project?.name ?? "範例專案",
          i: project?.startTime ?? "2026-01-01",
          j: project?.endTime ?? "2026-12-31",
          k: project?.companyName ?? "範例公司",
        };
      },
      "api/MbsWorkProject/GetManyMemberRole": () => ({
        aa: 1,
        a: [
          { a: 2 },
          { a: 3 },
          { a: 4 },
          { a: 5 },
        ],
      }),
      "api/MbsWorkProject/GetManyProjectStone": () => ({
        aa: 1,
        a: [],
        b: 0,
      }),
      "api/MbsWorkProject/GetManyProjectStoneTask": () => ({
        aa: 1,
        a: [],
        b: 0,
      }),
      "api/MbsWorkProject/GetManyExpense": () => ({
        aa: 1,
        a: [],
        b: [],
        c: [],
      }),

      "api/MbsSystemDepartment/GetMany": () => {
        const { items, totalCount } = getPaginatedData(
          mockDataSets.departments,
          pageIndex,
          pageSize
        );
        return {
          aa: 0,
          a: items,
          b: totalCount,
        };
      },

      "api/MbsSystemRole/GetMany": () => {
        const { items, totalCount } = getPaginatedData(
          mockDataSets.roles,
          pageIndex,
          pageSize
        );
        return {
          aa: 0,
          a: items,
          b: totalCount,
        };
      },

      "api/MbsCrmActivity/GetManyActivity": () => {
        const { items, totalCount } = getPaginatedData(
          mockDataSets.crmActivities,
          pageIndex,
          pageSize
        );
        return {
          aa: 1,
          a: items.map((item) => ({
            a: item.id,
            b: item.name,
            c: item.kind,
            d: item.startTime,
            e: item.endTime,
            f: item.location,
            g: item.expectedLeadCount,
            h: item.registeredCount,
            i: item.transferredToSalesCount,
            j: item.sponsorTotal,
            k: item.expenseTotal,
            l: item.opportunityCount,
          })),
          b: totalCount,
        };
      },

      "api/MbsCrmActivity/GetActivity": () => {
        const activityId = requestData?.a;
        const activity =
          mockDataSets.crmActivities.find((item) => item.id === activityId) ??
          mockDataSets.crmActivities[0];

        return {
          aa: 1,
          a: activity?.name ?? "範例活動",
          b: activity?.kind ?? 1,
          c: activity?.startTime ?? "2026-03-15T09:00:00",
          d: activity?.endTime ?? "2026-03-15T17:00:00",
          e: activity?.location ?? "台北國際會議中心",
          f: activity?.expectedLeadCount ?? 0,
          g: activity?.content ?? "活動內容",
          h: activity?.registeredCount ?? 0,
          i: activity?.transferredToSalesCount ?? 0,
          j: activity?.opportunityCount ?? 0,
          k: [
            {
              a: 1,
              b: 101,
              c: "雲端資安方案",
              d: 10,
              e: "資安",
              f: 101,
              g: "雲端防護",
            },
          ],
          l: [],
          m: [],
        };
      },

      "api/MbsCrmActivity/GetManyActivityProduct": () => ({
        aa: 1,
        a: [],
      }),

      "api/MbsCrmActivity/GetManyActivitySponsor": () => ({
        aa: 1,
        a: [],
      }),

      "api/MbsCrmActivity/GetManyActivityExpense": () => ({
        aa: 1,
        a: [],
      }),

      "api/MbsCrmPhoneSales/GetManyCustomer": () => {
        const { items, totalCount } = getPaginatedData(
          mockDataSets.phoneSalesCustomers,
          pageIndex,
          pageSize
        );
        return {
          a: 1,
          b: items.map((item) => ({
            a: item.id,
            b: item.companyName,
            c: item.industry,
            d: item.atomPhoneSalesCustomerStatus,
            e: item.atomPhoneSalesCustomerValue,
            f: item.atomPhoneSalesDataSource,
            g: item.isExistingCustomer,
            h: item.customOrder,
            i: item.contacterList.map((contact) => ({
              a: contact.contacterName,
              b: contact.contacterJobTitle,
              c: contact.contacterPhone,
              d: contact.contacterEmail,
            })),
          })),
          c: totalCount,
        };
      },

      "api/MbsCrmPhoneSales/GetCustomer": () => {
        const customerId = requestData?.a;
        const customer =
          mockDataSets.phoneSalesCustomers.find((item) => item.id === customerId) ??
          mockDataSets.phoneSalesCustomers[0];

        return {
          a: 1,
          b: customer?.id ?? 1,
          c: customer?.companyName ?? "範例公司",
          d: customer?.unifiedNumber ?? "12345678",
          e: customer?.industry ?? "資訊服務",
          f: customer?.companyAddress ?? "台北市信義區松仁路100號",
          g: customer?.companyPhone ?? "02-2345-6789",
          h: customer?.companyWebsite ?? "https://example.com",
          i: customer?.atomPhoneSalesCustomerStatus ?? 1,
          j: customer?.atomPhoneSalesCustomerValue ?? 2,
          k: customer?.atomPhoneSalesDataSource ?? 1,
          l: customer?.isExistingCustomer ?? false,
          m: customer?.remark ?? "",
          n:
            customer?.contacterList.map((contact) => ({
              a: contact.id,
              b: contact.contacterName,
              c: contact.contacterJobTitle,
              d: contact.contacterPhone,
              e: contact.contacterEmail,
              f: contact.contacterLineID,
              g: contact.isPrimary,
            })) ?? [],
        };
      },
      "api/MbsWorkJob/GetManyProjectStoneJob": () => {
        const { items, totalCount } = getPaginatedData(
          mockDataSets.workJobs,
          pageIndex,
          pageSize
        );
        return {
          aa: 1,
          a: items.map((item) => ({
            a: item.id,
            b: item.projectName,
            c: item.stoneName,
            d: item.jobName,
            e: item.status,
            f: item.startTime,
            g: item.endTime,
            h: item.executors.map((executorName) => ({ a: executorName })),
          })),
          b: totalCount,
        };
      },
      "api/MbsWorkJob/GetProjectStoneJob": () => {
        const jobId = requestData?.a;
        const job =
          mockDataSets.workJobs.find((item) => item.id === jobId) ?? mockDataSets.workJobs[0];
        const workList = mockDataSets.workJobRecords.filter((record) => record.jobId === job.id);
        const projectStart = `${job.startTime}T00:00:00`;
        const projectEnd = `${job.endTime}T23:59:59`;
        return {
          aa: 1,
          a: job.projectName,
          b: projectStart,
          c: projectEnd,
          d: job.stoneName,
          e: projectStart,
          f: projectEnd,
          g: job.jobName,
          h: `${job.startTime}T09:00:00`,
          i: `${job.endTime}T18:00:00`,
          j: job.status,
          k: "請依照里程碑完成工項內容與工作記錄。",
          l: job.executors.map((executorName) => ({ a: executorName })),
          m: [
            { a: 1, b: "需求確認", c: true },
            { a: 2, b: "規格撰寫", c: false },
            { a: 3, b: "審核與調整", c: false },
          ],
          n: workList.map((record) => ({
            a: record.id,
            b: record.startTime,
            c: record.endTime,
            d: record.remark,
            e: record.employeeName,
          })),
          o: [
            { a: "https://files.example.com/work-job/plan.pdf" },
            { a: "https://files.example.com/work-job/checklist.xlsx" },
          ],
        };
      },
      "api/MbsWorkJob/GetProjectStoneJobWork": () => {
        const workId = requestData?.a;
        const record =
          mockDataSets.workJobRecords.find((item) => item.id === workId) ??
          mockDataSets.workJobRecords[0];
        const job =
          mockDataSets.workJobs.find((item) => item.id === record.jobId) ?? mockDataSets.workJobs[0];
        const projectStart = `${job.startTime}T00:00:00`;
        const projectEnd = `${job.endTime}T23:59:59`;
        return {
          aa: 1,
          a: job.id,
          b: "工作內容備註示意。",
          c: [
            { a: 1, b: "需求確認", c: true },
            { a: 2, b: "規格撰寫", c: false },
          ],
          d: record.startTime,
          e: record.endTime,
          f: record.remark,
          g: [
            { a: 1, b: "https://files.example.com/work-job/work-log.pdf" },
            { a: 2, b: "https://files.example.com/work-job/meeting-notes.docx" },
          ],
          h: record.projectName,
          i: projectStart,
          j: projectEnd,
          k: record.stoneName,
          l: projectStart,
          m: projectEnd,
          n: record.jobName,
          o: `${job.startTime}T09:00:00`,
          p: `${job.endTime}T18:00:00`,
        };
      },
      "api/MbsWorkJob/GetManyProjectStoneJobWork": () => {
        const jobId = requestData?.c ?? null;
        const filtered =
          jobId !== null
            ? mockDataSets.workJobRecords.filter((record) => record.jobId === jobId)
            : mockDataSets.workJobRecords;
        const { items, totalCount } = getPaginatedData(filtered, pageIndex, pageSize);
        return {
          aa: 1,
          a: items.map((record) => ({
            a: record.id,
            b: record.startTime,
            c: record.endTime,
            d: record.projectName,
            e: record.stoneName,
            f: record.jobName,
            g: record.employeeName,
            h: record.remark,
            i: record.jobId,
          })),
          b: totalCount,
        };
      },
    };

    const normalizeSuccessCode = (result: any) => {
      if (
        result &&
        typeof result === "object" &&
        !Array.isArray(result) &&
        Object.prototype.hasOwnProperty.call(result, "aa") &&
        result.aa === 0
      ) {
        return { ...result, aa: 1 };
      }
      return result;
    };

    const mockDataGenerator = mockDataMap[normalizedEndpoint];
    if (mockDataGenerator) {
      const result = normalizeSuccessCode(mockDataGenerator());
      console.log(`[Mock API] ${normalizedEndpoint}`, result);
      return result as T;
    }

    console.warn(`[Mock API] No mock data defined for endpoint: ${normalizedEndpoint}`);
    const isListEndpoint = /GetMany|GetAll|List|GetPage|GetPaging|Query|Search/i.test(
      normalizedEndpoint
    );
    const isDetailEndpoint =
      /Get/i.test(normalizedEndpoint) && !/GetMany|GetAll|List|GetPage|GetPaging/i.test(normalizedEndpoint);
    if (isListEndpoint) {
      return {
        aa: 1,
        a: [],
        b: 0,
      } as T;
    }
    if (isDetailEndpoint) {
      return {
        aa: 1,
        a: {},
      } as T;
    }
    return {
      aa: 1,
    } as T;
  }
}

export const createMockApiClient = (config: {
  baseURL?: string;
  headers?: any;
}): AxiosInstance => {
  return new MockApiClient(config) as unknown as AxiosInstance;
};
