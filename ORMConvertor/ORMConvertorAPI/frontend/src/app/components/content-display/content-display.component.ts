import {
  Component,
  EventEmitter,
  Input,
  Output,
  ChangeDetectionStrategy,
  AfterViewInit,
  OnChanges,
  SimpleChanges,
  ViewChild,
  ElementRef,
} from "@angular/core";
import { FormsModule } from "@angular/forms";
import { ContentType } from "../../model/content-type";
@Component({
  selector: "app-content-display",
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule],
  templateUrl: "./content-display.component.html",
  styleUrls: ["./content-display.component.less"],
})
export class ContentDisplayComponent implements AfterViewInit, OnChanges {
  @Input() contentType: ContentType = ContentType.CSharpEntity;
  @Input() content: string = "";
  @Input() autoResize: boolean = false;
  @Input() description: string = "";
  @Output() contentChange = new EventEmitter<string>();
  @Input() readonly = false;
  @ViewChild('codeArea') private codeArea!: ElementRef<HTMLTextAreaElement>;

  ngAfterViewInit(): void {
    if (this.autoResize) {
      this.resize();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.autoResize && changes['content']) {
      setTimeout(() => this.resize(), 0);
    }
  }

  private resize(): void {
    if (!this.codeArea) {
      return;
    }
    const ta = this.codeArea.nativeElement;
    ta.style.height = 'auto';
    ta.style.height = `${ta.scrollHeight+25}px`;
  }
}
