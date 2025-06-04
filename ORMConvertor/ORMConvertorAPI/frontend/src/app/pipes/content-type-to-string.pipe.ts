import { Pipe, PipeTransform } from "@angular/core";
import { ContentType } from "../model/content-type";

@Pipe({
  name: "contentTypeToString",
})
export class ContentTypeToStringPipe implements PipeTransform {
  transform(value: ContentType): string {
    switch (value) {
      case ContentType.CSharpEntity:
        return "C# Entity";
      case ContentType.XML:
        return "XML";
        case ContentType.CSharpQuery:
          return "C# Query";
      default:
        return "Unknown";
    }
  }
}
