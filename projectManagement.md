# 文件目的

<div class="flex items-center flex-row gap-2 justify-between mb-3"><div class="flex flex-row gap-2 justify-between w-full"><h1 class="h1">專案管理</h1><button class="btn-add"> 新增專案 </button></div></div><div class="flex flex-col bg-white rounded-lg p-8 gap-4 h-full overflow-hidden"><div class="flex items-end gap-4"><div class="flex gap-2"><div><label class="query-label">專案狀態</label><select class="select-box"><option>全部</option><option value="1">未指派</option><option value="2">未開始</option><option value="3">如期</option><option value="4">注意</option><option value="5">延遲</option><option value="6">已完成</option></select></div><div><label class="query-label">專案名稱</label><input type="text" class="input-box" placeholder="專案名稱"></div></div><div><button class="btn-search me-1">查詢</button><button class="btn-cancel">清除</button></div></div><hr><div class="text-xs text-gray-500">點擊任一列即可進入專案詳情</div><div class="flex-1 overflow-y-auto table-scrollable"><table class="table-base table-fixed table-sticky w-full"><thead class="sticky top-0 bg-white z-10"><tr><th class="text-center w-16">專案狀態</th><th class="text-start w-40">專案名稱</th><th class="text-start w-36">客戶</th><th class="text-start w-24">起始日期</th><th class="text-start w-24">結束日期</th></tr></thead><tbody class="divide-y divide-gray-100"><tr class="text-center"><td colspan="5">無資料</td></tr></tbody></table></div><div class="flex justify-end items-end"><div class="flex items-center justify-center gap-4 p-2.5 rounded-lg"><!-- 每頁筆數選擇 --><div class="select-wrapper"><select class="select-box" value="10"><option value="10">10筆</option><option value="50">50筆</option><option value="100">100筆</option></select></div><div class="flex items-center gap-2"><!-- 上一頁 --><button class="flex items-center justify-center w-8 h-8 rounded bg-gray-200 hover:bg-cyan-500 transition-colors disabled:bg-gray-300 disabled:cursor-not-allowed" disabled=""><svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4 text-gray-700 hover:text-white transition-colors" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path stroke-linecap="round" stroke-linejoin="round" d="M15 19l-7-7 7-7"></path></svg></button><!-- 當前頁碼 --><span class="whitespace-nowrap text-sm text-gray-700">1 / 0</span><!-- 下一頁 --><button class="flex items-center justify-center w-8 h-8 rounded bg-gray-200 hover:bg-cyan-500 transition-colors disabled:bg-gray-300 disabled:cursor-not-allowed" disabled=""><svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4 text-gray-700 hover:text-white transition-colors" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"><path stroke-linecap="round" stroke-linejoin="round" d="M9 5l7 7-7 7"></path></svg></button></div><!-- 總筆數 --><span class="whitespace-nowrap text-sm text-gray-700">共 0 筆</span></div></div></div></div>去做修改與優化。

## 專案成員

› <div class="space-y-2"><div class="flex justify-between items-center"><label class="form-label">專案成員</
label><button class="btn-add">附加人員</button></div><span class="error-tip"> 請確保每一個角色至少有一位人員 </
span><!-- 成員列表表格 --><table class="table-base table-fixed w-full"><thead class="bg-gray-800 text-white"><tr><th
  class="text-start">層級角色</th><th class="text-start">地區</th><th class="text-start">部門</th><th class="text-
  start">人員</th><th class="text-center w-24">操作</th></tr></thead><tbody><!-- 總經理固定列 --><tr class="bg-gray-
  50"><td class="text-start">總經理</td><td class="text-start">全區</td><td class="text-start">總經理室</td><td
  class="text-start">陳建良</td><td class="text-center"><button class="btn-delete text-sm px-3 py-1" disabled="">刪除</
button></td></tr><tr><td class="text-start">專案經理</td><td class="text-start">全區</td><td class="text-start">策略執
行部</td><td class="text-start">巫若瑋</td><td class="text-center"><button class="btn-delete text-sm px-3 py-1"> 刪除
</button></td></tr></tbody></table></div>
專案成員部分，請不要有總經理，能是因為現有流程中如果沒有在專案成員裡面的話總
經理就無法看到此專案，但是不應該把總經理也列成專案成員，總經理本身就可以看到所有公司專案，因為他是公司負責人。因此專案

## 專案管理模組說明

### 目的

管理公司所有專案進度、狀態追蹤、人員配置才可以進而統計專案數量、風險等級以及人員量能查看、以及專案成本、跟統計公司營收。

### 專案來源

1.從商機來成案(狀態為 100%)後而來

- 商機 100%成立之後轉為專案，專案成員

  2.業務自己接到的專案

成員邏輯：
業務新增商機或是要轉成專案時，業務本身就是 default 值，並且會自動代入該負責商機之業務
