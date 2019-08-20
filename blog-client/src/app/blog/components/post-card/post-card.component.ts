import { Component, OnInit, Input } from '@angular/core';
import { Post } from '../../models/post';

@Component({
  selector: 'app-post-card',
  templateUrl: './post-card.component.html',
  styleUrls: ['./post-card.component.scss']
})
export class PostCardComponent implements OnInit {

  // usage: <app-post-card [post]="item"></app-post-card>
  @Input() post: Post;
  constructor() { }

  ngOnInit() {
  }

}
