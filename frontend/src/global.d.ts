// Add this to a declarations file like src/types.d.ts or src/global.d.ts
interface Window {
  ENV?: {
    BackendApi__Url?: string;
    [key: string]: any;
  }
}