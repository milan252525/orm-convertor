import { ORMType } from "./orm-type";
import { ContentType } from "./content-type";

export interface RequiredContentUnit {
  id: number;
  contentType: ContentType;
  description: string;
}

export interface RequiredContentDefinition {
  ormType: ORMType;
  required: RequiredContentUnit[];
}
