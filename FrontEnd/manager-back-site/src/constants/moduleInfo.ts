export interface ModuleInfo {
  title: string;
  source: string;
  description: string;
}

interface ModuleInfoRule {
  match: RegExp;
  info: ModuleInfo;
}

const moduleInfoRules: ModuleInfoRule[] = [
  {
    match: /^\/home/,
    info: {
      title: "Dashboard",
      source: "mockDataSets + buildDashboardMockData + localStorage cache.dashboard.*",
      description: "Role-based dashboard metrics and cards for mock demo.",
    },
  },
  {
    match: /^\/work\/project\/assign/,
    info: {
      title: "Project Assignments",
      source: "localStorage cache.work.project.assignments (seeded in Sidebar/Assign)",
      description: "Pending assignment list used for internal routing demo.",
    },
  },
  {
    match: /^\/work\/project\/change/,
    info: {
      title: "Project Change",
      source: "localStorage cache.work.project.changeRequests",
      description: "Mock change requests and file review flows.",
    },
  },
  {
    match: /^\/work\/project/,
    info: {
      title: "Project Management",
      source: "mockDataSets.workProjects + cache.work.project.* + projectTemplateSettings",
      description: "Project list/detail and derived POC info in mock mode.",
    },
  },
  {
    match: /^\/work\/capacity/,
    info: {
      title: "Capacity",
      source: "mockDataStore certificates + localStorage cache.work.capacity",
      description: "Capacity/skills display backed by mock seed and local cache.",
    },
  },
  {
    match: /^\/work\/job/,
    info: {
      title: "Work Jobs",
      source: "mockDataSets.workJobs + mockDataSets.workJobRecords",
      description: "Work job list/detail seeded for demo.",
    },
  },
  {
    match: /^\/crm\/pipeline\/handoff/,
    info: {
      title: "Pipeline Handoff",
      source: "localStorage cache.crm.pipeline.handoff.*",
      description: "Mock handoff list and detail stored locally.",
    },
  },
  {
    match: /^\/crm\/pipeline\/pipeline/,
    info: {
      title: "CRM Pipeline",
      source: "mockDataSets.crmPipelines + cache.crm.pipeline.*",
      description: "Pipeline list/detail and POC data for mock sales flow.",
    },
  },
  {
    match: /^\/crm\/phone\/pool/,
    info: {
      title: "Phone Lead Pool",
      source: "localStorage cache.crm.phone.leadPool",
      description: "Lead pool data created from phone activity lists.",
    },
  },
  {
    match: /^\/crm\/phone\/activity/,
    info: {
      title: "Phone Activities",
      source: "mockDataSets.crmActivities + mockDataSets.teleSalesRecords",
      description: "Phone activity list and pipeline records for mock demo.",
    },
  },
  {
    match: /^\/crm\/phone-sales\/customer/,
    info: {
      title: "Phone Sales Customers",
      source: "mockDataSets.phoneSalesCustomers",
      description: "Phone-sales customer list and details.",
    },
  },
  {
    match: /^\/crm\/activity\/activity/,
    info: {
      title: "CRM Activities",
      source: "mockDataSets.crmActivities",
      description: "Activity list/detail and linked pipeline entries.",
    },
  },
  {
    match: /^\/system\/master/,
    info: {
      title: "System Master Data",
      source: "mockDataSets + localStorage cache.system.product.*",
      description: "Master data lists for company/contact/product/employee.",
    },
  },
  {
    match: /^\/system\/project-template/,
    info: {
      title: "Project Templates",
      source: "projectTemplateSettings store + localStorage cache.projectTemplateSettings",
      description: "Template stages and outputs for work projects.",
    },
  },
];

export const getModuleInfoByPath = (path: string): ModuleInfo | null => {
  const matched = moduleInfoRules.find((rule) => rule.match.test(path));
  return matched?.info ?? null;
};
