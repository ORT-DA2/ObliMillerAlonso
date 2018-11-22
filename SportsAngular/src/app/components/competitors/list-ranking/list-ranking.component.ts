import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-list-ranking',
  templateUrl: './list-ranking.component.html',
  styleUrls: ['./list-ranking.component.css']
})
export class ListRankingComponent {
  @Input() competitors: any;
  constructor() { }


}
