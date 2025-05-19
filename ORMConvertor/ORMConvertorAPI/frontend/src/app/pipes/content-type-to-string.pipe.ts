import { Pipe, PipeTransform } from "@angular/core";
import { ContentType } from "../model/content-type";

@Pipe({
  name: "contentTypeToString",
})
export class ContentTypeToStringPipe implements PipeTransform {
  transform(value: ContentType): string {
    switch (value) {
      case ContentType.CSharp:
        return "C#";
      case ContentType.XML:
        return "XML";
      default:
        return "Unknown";
    }
  }
}
