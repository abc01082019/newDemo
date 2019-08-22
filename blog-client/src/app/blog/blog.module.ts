import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BlogRoutingModule } from './blog-routing.module';
import { MaterialModule } from '../shared/material/material.module';
import { BlogAppComponent } from './blog-app.component';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { PostService } from './services/post.service';
import { PostListComponent } from './components/post-list/post-list.component';
import { AuthorizationHeaderInterceptor } from '../shared/oidc/authorization-header-interceptor.interceptor';
import { PostCardComponent } from './components/post-card/post-card.component';
import { WritePostComponent } from './components/write-post/write-post.component';
import { TinymceService } from './services/tinymce.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { EditorModule } from '@tinymce/tinymce-angular';
import { EnsureAcceptHeaderInterceptor } from '../shared/ensure-accept-header-interceptor';
import { EditPostComponent } from './components/edit-post/edit-post.component';
import { PostDetailComponent } from './components/post-detail/post-detail.component';
import { SafeHtmlPipe } from '../shared/safe-html.pipe';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { PostTableComponent } from './components/post-table/post-table.component';

@NgModule({
  declarations: [
    BlogAppComponent, 
    SidenavComponent, 
    ToolbarComponent, 
    PostListComponent, 
    PostCardComponent, 
    WritePostComponent,
    EditPostComponent,
    PostDetailComponent,
    PostTableComponent,
    SafeHtmlPipe,
    PostTableComponent
  ],
  imports: [
    CommonModule,
    BlogRoutingModule,
    HttpClientModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    EditorModule,
    InfiniteScrollModule
  ],
  providers: [
    PostService,
    TinymceService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthorizationHeaderInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: EnsureAcceptHeaderInterceptor,
      multi: true
    }
  ]
})
export class BlogModule { }
