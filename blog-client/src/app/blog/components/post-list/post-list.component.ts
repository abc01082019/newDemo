import { Component, OnInit } from '@angular/core';
import { PostService } from '../../services/post.service';
import { PostParameters } from '../../models/post-parameters';
import { PageMeta } from 'src/app/shared/models/page-meta';
import { ResultWithLinks } from 'src/app/shared/models/result_with_links';
import { Post } from '../../models/post';
import { OpenIdConnectService } from 'src/app/shared/oidc/open-id-connect.service';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.scss']
})
export class PostListComponent implements OnInit {

  private posts: Post[];
  private pageMeta: PageMeta;
  postParameter = new PostParameters({
    orderBy: 'id desc',
    pageSize: 10,
    pageIndex: 0
  });

  constructor(private postService: PostService, 
    private openIdConnectService: OpenIdConnectService) { }

  ngOnInit() {
    this.posts= [];
    this.getPosts();
  }

  getPosts() {
    // subscribe return all the infomation from response
    this.postService.getPagedPosts(this.postParameter).subscribe(resp =>{
      this.pageMeta = JSON.parse(resp.headers.get("X-pagination")) as PageMeta;
      let result = {...resp.body} as ResultWithLinks<Post>;
      this.posts = this.posts.concat(result.value);
    });
  }

  onScroll() {
    console.log('scrolled down!!');
    this.postParameter.pageIndex++;
    if (this.postParameter.pageIndex < this.pageMeta.pageCount) {
      this.getPosts();
    }
  }
}
