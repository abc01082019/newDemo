import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router, ParamMap } from '@angular/router';
import { MatSnackBar } from '@angular/material';
import { compare } from 'fast-json-patch';
import { ValidationErrorHandler } from '../../../shared/validation-error-handler';
import { PostService } from '../../services/post.service';
import { TinymceService } from '../../services/tinymce.service';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-edit-post',
  templateUrl: './edit-post.component.html',
  styleUrls: ['./edit-post.component.scss']
})
export class EditPostComponent implements OnInit {

  post: any;
  postForm: FormGroup;
  editorSettings;
  id: string | number;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private postService: PostService,
    private tinymce: TinymceService,
    private fb: FormBuilder,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit() {
    this.route.paramMap.pipe(
      switchMap((params: ParamMap) => {
        this.id = params.get('id');
        return this.postService.getPostById(this.id);
      })
    ).subscribe(post => {
      this.post = { title: post.title, body: post.body };

      this.postForm = this.fb.group({
        title: [this.post.title, [Validators.required, Validators.minLength(1), Validators.maxLength(100)]],
        body: [this.post.body, [Validators.required, Validators.minLength(1)]]
      });
    });

    this.editorSettings = this.tinymce.getSettings();
  }

  save() {
    if (this.postForm.dirty && this.postForm.valid) {
      const patchDocument = compare(this.post, this.postForm.value);

      this.postService.partiallyUpdatePost(this.id, patchDocument).subscribe(
        () => {
          this.router.navigate(['/blog/post-list/', this.id]);
        },
        validationResult => {
          this.snackBar.open('There are validation errors!', 'Close', { duration: 3000 });
          ValidationErrorHandler.handleFormValidationErrors(this.postForm, validationResult);
        });
    }
  }
}
