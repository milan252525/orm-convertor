import { CommonModule, KeyValuePipe, Location } from "@angular/common";
import { Component, DestroyRef, inject, OnInit, AfterViewInit, HostListener, ElementRef } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { FormsModule } from "@angular/forms";
import { finalize, of } from "rxjs";
import { delay } from "rxjs/operators";
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
import { SAMPLES } from "../../model/samples";

@Component({
  selector: "app-demo-page",
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ContentDisplayComponent,
    ContentTypeToStringPipe,
    ResultTableComponent,
  ],
  templateUrl: "./demo-page.component.html",
  styleUrls: ["./demo-page.component.less"],
})
export class DemoPageComponent implements OnInit, AfterViewInit {
  private destroyRef = inject(DestroyRef);

  ormTypeEnum = ORMType;

  showResults = false;

  /**
   * Filtered list of ORM options (only enum names, excluding numeric reverse mappings).
   */
  readonly ormTypeOptions: { key: string; value: ORMType }[] = Object.keys(
    ORMType
  )
    .filter((k) => isNaN(Number(k)))
    .map((k) => ({ key: k, value: (ORMType as any)[k] as ORMType }));
  contentTypeEnum = ContentType;

  isLoading = false;
  // Animated dots for optimizing indicator
  loadingDots: string = "";
  private loadingInterval?: any;

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

  constructor(private ormService: OrmService, private elRef: ElementRef, private location: Location) {}

  ngOnInit(): void {
    this.ormService
      .getRequiredContentAdvisor()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((required) => {
        this.requiredContent = required;
        this.updateRequiredUnits();
      });

    this.ormService
      .getSamples()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((samples) => {
        // this.samples = new Map(
        //   Object.entries(samples).map(([k, v]) => [Number(k), v as string])
        // );
        this.samples = new Map();
        this.samples.set(4, SAMPLES.entity);
        this.samples.set(5, SAMPLES.query1);
        this.samples.set(6, SAMPLES.query2);
        this.samples.set(7, SAMPLES.query3);
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
    // start dots animation: one dot per second up to three, then reset
    this.loadingDots = "";
    this.loadingInterval = setInterval(() => {
      if (this.loadingDots.length < 6) {
        this.loadingDots += ".";
      } else {
        this.loadingDots = "";
      }
    }, 500);
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
    // use fake observable instead of real service
    of(null)
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        // artificial delay before completing
        delay(15000),
        finalize(() => {
          this.isLoading = false;
          this.showResults = true;
          // stop dots animation
          if (this.loadingInterval) {
            clearInterval(this.loadingInterval);
            this.loadingInterval = undefined;
          }
          this.loadingDots = "";
          // ensure textareas are resized after content is rendered
          setTimeout(() => this.resizeAll(), 0);
        })
      )
      .subscribe({
        next: (r) => {
          // this.convertedUnits = r.sources;
          // demo content
          this.convertedUnits = [
            {
              contentType: ContentType.CSharpEntity,
              content: SAMPLES.entityTarget,
            },
            {
              contentType: ContentType.CSharpQuery,
              content: SAMPLES.tquery1,
            },
            {
              contentType: ContentType.CSharpQuery,
              content: SAMPLES.tquery2,
            },
            {
              contentType: ContentType.CSharpQuery,
              content: SAMPLES.tquery3,
            },
          ];
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
      this.resizeAll();
    });
    // resize after filling samples
    setTimeout(() => this.resizeAll(), 0);
  }

  /**
   * Visually checks all target framework checkboxes.
   * @param container - The container element holding the checkboxes
   */
  selectAllTargets(container: HTMLElement): void {
    const boxes = container.querySelectorAll(
      'input[type="checkbox"]'
    ) as NodeListOf<HTMLInputElement>;
    boxes.forEach((b) => (b.checked = true));
  }
  /**
   * Auto-resize textareas to fit content, no vertical scroll.
   */
  @HostListener('input', ['$event'])
  onInput(event: Event): void {
    const target = event.target as HTMLTextAreaElement;
    if (target && target.tagName.toLowerCase() === 'textarea' && target.classList.contains('code-area')) {
      this.resizeTextArea(target);
    }
  }

  ngAfterViewInit(): void {
    this.resizeAll();
  }

  /**
   * Resize a single textarea to fit its content.
   */
  private resizeTextArea(textarea: HTMLTextAreaElement): void {
    textarea.style.height = 'auto';
    textarea.style.height = `${textarea.scrollHeight}px`;
  }

  /**
   * Resize all code-area textareas within this component.
   */
  private resizeAll(): void {
    const areas: NodeListOf<HTMLTextAreaElement> = this.elRef.nativeElement.querySelectorAll('textarea.code-area');
    areas.forEach((ta) => this.resizeTextArea(ta));
  }
  
  /** Navigate back to previous page */
  back(): void {
    this.location.back();
  }
}
