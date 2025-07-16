import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ConvertRequest, ConvertResponse } from "../model/convert";
import { RequiredContentDefinition } from "../model/required-content";

@Injectable({ providedIn: "root" })
export class OrmService {
  constructor(private http: HttpClient) {}

  getRequiredContent(): Observable<RequiredContentDefinition[]> {
    return this.http.get<RequiredContentDefinition[]>("/required-content");
  }

  getSamples(): Observable<Record<number, string>> {
    return this.http.get<Record<number, string>>("/samples");
  }

  convert(req: ConvertRequest): Observable<ConvertResponse> {
    return this.http.post<ConvertResponse>("/convert", req);
  }
}
