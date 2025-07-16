import { ChangeDetectionStrategy, Component } from "@angular/core";
@Component({
  selector: "app-result-table",
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [],
  templateUrl: "./result-table.component.html",
  styleUrls: ["./result-table.component.less"],
  standalone: true,
})
export class ResultTableComponent {}
