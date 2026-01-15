import axios from "axios";
import { useTokenStore } from "../stores/token";
import { createMockApiClient } from "./mockApi/mockApiClient";

const useMockData = true;
const cachePrefix = "cache.api";

const safeStringify = (value: unknown) => {
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
};

const buildCacheKey = (config: { method?: string; url?: string; data?: any; params?: any }) => {
  const method = (config.method || "get").toLowerCase();
  const rawUrl = config.url || "";
  const url = rawUrl.startsWith("/") ? rawUrl.slice(1) : rawUrl;
  const payload = config.data ?? config.params ?? {};
  return `${cachePrefix}:${method}:${url}:${safeStringify(payload)}`;
};

const writeCache = (config: { method?: string; url?: string; data?: any; params?: any }, data: unknown) => {
  const key = buildCacheKey(config);
  localStorage.setItem(key, safeStringify(data));
};

const readCache = (config: { method?: string; url?: string; data?: any; params?: any }) => {
  const key = buildCacheKey(config);
  const raw = localStorage.getItem(key);
  if (!raw) return null;
  try {
    return JSON.parse(raw);
  } catch {
    return null;
  }
};

// JSON請求
const mockJsonClient = createMockApiClient({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});
const realJsonClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

export const apiJsonClient = useMockData ? mockJsonClient : realJsonClient;

// 請求攔截
realJsonClient.interceptors.request.use(
  (config) => {
    const token = useTokenStore();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// 回應攔截
const callMockClient = (client: typeof mockJsonClient, config: any) => {
  const method = (config?.method || "get").toLowerCase();
  switch (method) {
    case "post":
      return client.post(config.url, config.data, config);
    case "put":
      return client.put(config.url, config.data, config);
    case "delete":
      return client.delete(config.url, config);
    case "get":
    default:
      return client.get(config.url, config);
  }
};

realJsonClient.interceptors.response.use(
  (response) => {
    writeCache(response.config, response.data);
    return response;
  },
  (error) => {
    if (error.response) {
      if (error.response.status == 401) {
        const tokenStore = useTokenStore();
        tokenStore.removeToken();
        window.location.href = "/login";
      }
    }
    const cached = readCache(error.config);
    if (cached) {
      return Promise.resolve({
        data: cached,
        status: 200,
        statusText: "OK",
        headers: {},
        config: error.config,
      });
    }
    return callMockClient(mockJsonClient, error.config)
      .then((mockResponse) => {
        writeCache(error.config, mockResponse.data);
        return mockResponse;
      })
      .catch(() => Promise.reject(error));
  }
);

// 表單請求
const mockFormClient = createMockApiClient({
  baseURL: import.meta.env.VITE_API_BASE_URL,
});
const realFormClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
});
export const apiFormClient = useMockData ? mockFormClient : realFormClient;
realFormClient.interceptors.request.use((config) => {
  const tokenStore = useTokenStore();
  if (tokenStore.token) {
    config.headers.Authorization = `Bearer ${tokenStore.token}`;
  }
  return config;
});

realFormClient.interceptors.response.use(
  (response) => {
    writeCache(response.config, response.data);
    return response;
  },
  (error) => {
    if (error.response?.status === 401) {
      const tokenStore = useTokenStore();
      tokenStore.removeToken();
      window.location.href = "/login";
    }
    const cached = readCache(error.config);
    if (cached) {
      return Promise.resolve({
        data: cached,
        status: 200,
        statusText: "OK",
        headers: {},
        config: error.config,
      });
    }
    return callMockClient(mockFormClient, error.config)
      .then((mockResponse) => {
        writeCache(error.config, mockResponse.data);
        return mockResponse;
      })
      .catch(() => Promise.reject(error));
  }
);
