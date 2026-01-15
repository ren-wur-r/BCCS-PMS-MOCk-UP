export interface MockAnnotationNote {
  id: string;
  routePath: string;
  selector: string;
  x?: number;
  y?: number;
  text: string;
}

export const mockAnnotations: MockAnnotationNote[] = [
  {
    id: "seed-project-template-service-items",
    routePath: "/system/project-template",
    selector: ".subtitle",
    text: "服務項目可新增/調整，並影響後續標準階段與產品清單。",
  },
];
