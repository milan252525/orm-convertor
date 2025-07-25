import {
  Component,
  DestroyRef,
  inject,
  OnDestroy,
  OnInit,
} from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { ContentDisplayComponent } from "./components/content-display/content-display.component";
import { ORMType } from "./model/orm-type";
import { ContentType } from "./model/content-type";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { ContentTypeToStringPipe } from "./pipes/content-type-to-string.pipe";

interface SourceUnit {
  contentType: ContentType;
  content: string;
}
interface ConvertRequest {
  sourceOrm: ORMType;
  targetOrm: ORMType;
  sources: SourceUnit[];
}
interface ConvertResponse {
  sources: SourceUnit[];
}
interface RequiredContentUnit {
  id: number;
  contentType: ContentType;
  description: string;
}
interface RequiredContentDefinition {
  ormType: ORMType;
  required: RequiredContentUnit[];
}

@Component({
  selector: "app-root",
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ContentDisplayComponent,
    ContentTypeToStringPipe,
  ],
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.less"],
})
export class AppComponent implements OnInit {
  private destroyRef = inject(DestroyRef);

  ormTypeEnum = ORMType;
  contentTypeEnum = ContentType;

  sourceOrm: ORMType = ORMType.EFCore;
  targetOrm: ORMType = ORMType.Dapper;
  text = "";
  result = "";
  error = "";
  convertedUnits: SourceUnit[] = [];

  requiredContent: RequiredContentDefinition[] = [];
  displayUnits: RequiredContentUnit[] = [];
  contentByUnit: { [unitId: string]: string } = {};

  samples: Map<number, string> = new Map();

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http
      .get<RequiredContentDefinition[]>("/orm/required-content")
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((required) => {
        this.requiredContent = required;
        this.updateRequiredUnits();
      });

    this.http
      .get<Map<number, string>>("/orm/samples")
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((samples) => {
        this.samples = new Map(
          Object.entries(samples).map(([k, v]) => [Number(k), v as string])
        );
      });
  }

  onSourceOrmChange(newOrm: string) {
    this.sourceOrm = +newOrm as ORMType;
    this.updateRequiredUnits();
  }

  onTargetOrmChange(newOrm: string){
    this.targetOrm = +newOrm as ORMType;
    this.convertedUnits = [];
  }

  private updateRequiredUnits() {
    this.displayUnits = [
      ...(this.requiredContent.find((r) => r.ormType === this.sourceOrm)
        ?.required ?? []),
    ];

    this.displayUnits.forEach((u) => {
      if (!(u.id in this.contentByUnit)) {
        this.contentByUnit[u.id] = "";
      }
    });
  }

  convert(): void {
    const body: ConvertRequest = {
      sourceOrm: this.sourceOrm,
      targetOrm: this.targetOrm,
      sources: this.displayUnits.map((u) => ({
        contentType: u.contentType,
        content: this.contentByUnit[u.id]!,
      })),
    };

    this.result = "Converting…";
    this.error = "";
    this.convertedUnits = [];
    this.http
      .post<ConvertResponse>("/orm/convert", body)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (r) => {
          this.convertedUnits = r.sources;
          this.result = "";
        },
        error: (err) => (this.error = err.message),
      });
  }

  fillWithSamples(): void {
    this.displayUnits.forEach((u) => {
      const sample = this.samples.get(u.id);
      if (sample !== undefined) {
        this.contentByUnit[u.id] = sample;
      }
    });
  }
}
