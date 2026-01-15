# 組織結構定義

說明：

- org_node_id：組織節點唯一 ID（需穩定，後續服務、權限、流程都會用）
- org_node_name：組織中文名稱
- parent_org_node_id：上層組織節點 ID（最上層為 null）
- org_path：由上而下的完整組織路徑

---

## Org Node: CEO

- org_node_id: CEO
- org_node_name: 總經理
- parent_org_node_id: null
- org_path: 總經理

---

## Org Node: FIN

- org_node_id: FIN
- org_node_name: 財務處
- parent_org_node_id: CEO
- org_path: 總經理 / 財務處

---

## Org Node: HR

- org_node_id: HR
- org_node_name: 人資行政處
- parent_org_node_id: CEO
- org_path: 總經理 / 人資行政處

### Sub Org Nodes

- org_node_id: HR_N
  org_node_name: 北區行政
  parent_org_node_id: HR
  org_path: 總經理 / 人資行政處 / 北區行政

- org_node_id: HR_C
  org_node_name: 中區行政
  parent_org_node_id: HR
  org_path: 總經理 / 人資行政處 / 中區行政

- org_node_id: HR_S
  org_node_name: 南區行政
  parent_org_node_id: HR
  org_path: 總經理 / 人資行政處 / 南區行政

---

## Org Node: RISK

- org_node_id: RISK
- org_node_name: 風險稽核處
- parent_org_node_id: CEO
- org_path: 總經理 / 風險稽核處

### Sub Org Nodes

- org_node_id: LAB
  org_node_name: 安全實驗室
  parent_org_node_id: RISK
  org_path: 總經理 / 風險稽核處 / 安全實驗室

- org_node_id: TS_N
  org_node_name: 北區技服部
  parent_org_node_id: RISK
  org_path: 總經理 / 風險稽核處 / 北區技服部

- org_node_id: TS_C
  org_node_name: 中區技服部
  parent_org_node_id: RISK
  org_path: 總經理 / 風險稽核處 / 中區技服部

- org_node_id: TS_S
  org_node_name: 南區技服部
  parent_org_node_id: RISK
  org_path: 總經理 / 風險稽核處 / 南區技服部

---

## Org Node: CONS

- org_node_id: CONS
- org_node_name: 管理顧問處
- parent_org_node_id: CEO
- org_path: 總經理 / 管理顧問處

### Sub Org Nodes

- org_node_id: CONS_A
  org_node_name: 北區顧問部
  parent_org_node_id: CONS
  org_path: 總經理 / 管理顧問處 / 北區顧問部

- org_node_id: CONS_B
  org_node_name: 中區顧問部
  parent_org_node_id: CONS
  org_path: 總經理 / 管理顧問處 / 中區顧問部

- org_node_id: CONS_C
  org_node_name: 南區顧問部
  parent_org_node_id: CONS
  org_path: 總經理 / 管理顧問處 / 南區顧問部

---

## Org Node: IT

- org_node_id: IT
- org_node_name: 資訊處
- parent_org_node_id: CEO
- org_path: 總經理 / 資訊處

### Sub Org Nodes

- org_node_id: IT_M
  org_node_name: 系統維護部
  parent_org_node_id: IT
  org_path: 總經理 / 資訊處 / 系統維護部

- org_node_id: IT_D
  org_node_name: 系統開發部
  parent_org_node_id: IT
  org_path: 總經理 / 資訊處 / 系統開發部

---

## Org Node: BIZ

- org_node_id: BIZ
- org_node_name: 業務行銷事業單位
- parent_org_node_id: CEO
- org_path: 總經理 / 業務行銷事業單位

### Sub Org Nodes: 行銷體系

- org_node_id: MKT
  org_node_name: 行銷處
  parent_org_node_id: BIZ
  org_path: 總經理 / 業務行銷事業單位 / 行銷處

- org_node_id: PLAN
  org_node_name: 企劃部
  parent_org_node_id: MKT
  org_path: 總經理 / 業務行銷事業單位 / 行銷處 / 企劃部

- org_node_id: PROJ
  org_node_name: 專案部
  parent_org_node_id: MKT
  org_path: 總經理 / 業務行銷事業單位 / 行銷處 / 專案部

- org_node_id: COOP
  org_node_name: 協銷部
  parent_org_node_id: MKT
  org_path: 總經理 / 業務行銷事業單位 / 行銷處 / 協銷部

### Sub Org Nodes: 區域業務體系

- org_node_id: B_A
  org_node_name: 北區業務處
  parent_org_node_id: BIZ
  org_path: 總經理 / 業務行銷事業單位 / 北區業務處

- org_node_id: B_A_SALES
  org_node_name: 業務部
  parent_org_node_id: B_A
  org_path: 總經理 / 業務行銷事業單位 / 北區業務處 / 業務部

- org_node_id: B_A_ENG
  org_node_name: 工程部
  parent_org_node_id: B_A
  org_path: 總經理 / 業務行銷事業單位 / 北區業務處 / 工程部

- org_node_id: B_B
  org_node_name: 中區業務處
  parent_org_node_id: BIZ
  org_path: 總經理 / 業務行銷事業單位 / 中區業務處

- org_node_id: B_B_SALES
  org_node_name: 業務部
  parent_org_node_id: B_B
  org_path: 總經理 / 業務行銷事業單位 / 中區業務處 / 業務部

- org_node_id: B_B_ENG
  org_node_name: 工程部
  parent_org_node_id: B_B
  org_path: 總經理 / 業務行銷事業單位 / 中區業務處 / 工程部

- org_node_id: B_C
  org_node_name: 南區業務處
  parent_org_node_id: BIZ
  org_path: 總經理 / 業務行銷事業單位 / 南區業務處

- org_node_id: B_C_SALES
  org_node_name: 業務部
  parent_org_node_id: B_C
  org_path: 總經理 / 業務行銷事業單位 / 南區業務處 / 業務部

- org_node_id: B_C_ENG
  org_node_name: 工程部
  parent_org_node_id: B_C
  org_path: 總經理 / 業務行銷事業單位 / 南區業務處 / 工程部

---

## Org Node: RND

- org_node_id: RND
- org_node_name: 創新服務研發處
- parent_org_node_id: CEO
- org_path: 總經理 / 創新服務研發處

### Sub Org Nodes

- org_node_id: STRAT
  org_node_name: 策略執行部
  parent_org_node_id: RND
  org_path: 總經理 / 創新服務研發處 / 策略執行部

- org_node_id: DEV
  org_node_name: 研發部
  parent_org_node_id: RND
  org_path: 總經理 / 創新服務研發處 / 研發部

- org_node_id: MON
  org_node_name: 監控部
  parent_org_node_id: RND
  org_path: 總經理 / 創新服務研發處 / 監控部

- org_node_id: AI
  org_node_name: AI 智能部
  parent_org_node_id: RND
  org_path: 總經理 / 創新服務研發處 / AI 智能部

- org_node_id: FORE
  org_node_name: 鑑識部
  parent_org_node_id: RND
  org_path: 總經理 / 創新服務研發處 / 鑑識部
