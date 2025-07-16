import { ContentType } from "./content-type";
import { ORMType } from "./orm-type";

export interface SourceUnit {
  contentType: ContentType;
  content: string;
}

export interface ConvertRequest {
  sourceOrm: ORMType;
  targetOrm: ORMType;
  sources: SourceUnit[];
}

export interface ConvertResponse {
  sources: SourceUnit[];
}
