import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ContentType } from '../../model/content-type';
@Component({
  selector: 'app-content-display',
  imports: [FormsModule],
  templateUrl: "./content-display.component.html",
  styleUrls: ["./content-display.component.less"],
})
export class ContentDisplayComponent {
  @Input() contentType: ContentType = ContentType.CSharpEntity;
  @Input() content: string = "";
  @Input() description: string = "";
  @Output() contentChange = new EventEmitter<string>();
  @Input() readonly = false; 
}
