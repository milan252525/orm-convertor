import { CommonModule, KeyValuePipe } from "@angular/common";
import { Component, DestroyRef, inject, OnInit } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { FormsModule } from "@angular/forms";
import { finalize } from "rxjs";
import { ContentDisplayComponent } from "../../components/content-display/content-display.component";
import { ContentType } from "../../model/content-type";
import { ConvertRequest, SourceUnit } from "../../model/convert";
import { ORMType } from "../../model/orm-type";
import {
  RequiredContentDefinition,
  RequiredContentUnit,
} from "../../model/required-content";
import { ContentTypeToStringPipe } from "../../pipes/content-type-to-string.pipe";
import { OrmService } from "../../services/orm.service";
import { ResultTableComponent } from "../../components/result-table/result-table.component";

@Component({
  selector: "app-demo-page",
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ContentDisplayComponent,
    ContentTypeToStringPipe,
    ResultTableComponent
  ],
  templateUrl: "./demo-page.component.html",
  styleUrls: ["./demo-page.component.less"],
})
export class DemoPageComponent implements OnInit {
  private destroyRef = inject(DestroyRef);

  ormTypeEnum = ORMType;
  
  /**
   * Filtered list of ORM options (only enum names, excluding numeric reverse mappings).
   */
  readonly ormTypeOptions: { key: string; value: ORMType }[] = Object.keys(ORMType)
    .filter(k => isNaN(Number(k)))
    .map(k => ({ key: k, value: (ORMType as any)[k] as ORMType }));
  contentTypeEnum = ContentType;

  isLoading = false;

  sourceOrm: ORMType = ORMType.EFCore;
  targetOrm: ORMType = ORMType.Dapper;
  // Selected target ORMs for conversion (multiple selection)
  targetOrms: ORMType[] = [];
  text = "";
  result = "";
  error = "";
  convertedUnits: SourceUnit[] = [];

  requiredContent: RequiredContentDefinition[] = [];
  displayUnits: RequiredContentUnit[] = [];
  contentByUnit: { [unitId: string]: string } = {};

  samples: Map<number, string> = new Map();

  constructor(private ormService: OrmService) {}

  ngOnInit(): void {
    this.ormService
      .getRequiredContent()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((required) => {
        this.requiredContent = required;
        this.updateRequiredUnits();
      });

    this.ormService
      .getSamples()
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

  onTargetOrmChange(newOrm: string) {
    this.targetOrm = +newOrm as ORMType;
    this.convertedUnits = [];
  }
  
  /**
   * Toggle selection of a target ORM framework.
   * @param ormValue - The ORM type value
   * @param checked - Whether the checkbox is checked
   */
  onTargetOrmToggle(ormValue: ORMType, checked: boolean): void {
    // Logic for handling multiple target ORM selection to be implemented
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
    this.isLoading = true;
    const body: ConvertRequest = {
      sourceOrm: this.sourceOrm,
      targetOrm: this.targetOrm,
      sources: this.displayUnits.map((u) => ({
        contentType: u.contentType,
        content: this.contentByUnit[u.id]!,
      })),
    };

    this.error = "";
    this.convertedUnits = [];
    this.ormService
      .convert(body)
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        finalize(() => (this.isLoading = false))
      )
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

  /**
   * Visually checks all target framework checkboxes.
   * @param container - The container element holding the checkboxes
   */
  selectAllTargets(container: HTMLElement): void {
    const boxes = container.querySelectorAll('input[type="checkbox"]') as NodeListOf<HTMLInputElement>;
    boxes.forEach(b => b.checked = true);
  }
}
