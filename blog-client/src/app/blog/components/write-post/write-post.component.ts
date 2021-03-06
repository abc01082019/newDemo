import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TinymceService } from '../../services/tinymce.service';
import { PostService } from '../../services/post.service';
import { MatSnackBar } from '@angular/material';
import { post } from 'selenium-webdriver/http';
import { ValidationErrorHandler } from 'src/app/shared/validation-error-handler';

@Component({
  selector: 'app-write-post',
  templateUrl: './write-post.component.html',
  styleUrls: ['./write-post.component.scss']
})
export class WritePostComponent implements OnInit {

  editorSettings: any;

  postForm: FormGroup;

  constructor(
    private router: Router,
    private postService: PostService,
    private tinymce: TinymceService,
    private fb: FormBuilder,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit() {
    this.postForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(100)]],
      body: ['', [Validators.required, Validators.minLength(1)]]
    });

    this.editorSettings = this.tinymce.getSettings();
  }

  submit() {
    if (this.postForm.dirty && this.postForm.valid) {
      this.postService.addPost(this.postForm.value).subscribe(
        post => {
          this.router.navigate(['/blog/posts/', post.id]);
        },
        validationResult => {
          this.snackBar.open('An error ocurred: validation error', 'Close', { duration: 3000 });
          ValidationErrorHandler.handleFormValidationErrors(this.postForm, validationResult);
        }
      );
    }
  }

}
