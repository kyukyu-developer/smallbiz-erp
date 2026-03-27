import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { LoginUseCase, LoginResult } from '../../features/auth/application/usecases/auth.usecase';
import { AUTH_REPOSITORY } from '../../core/interfaces/repositories/repository-tokens';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatCheckboxModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  private formBuilder = inject(FormBuilder);
  private loginUseCase = inject(LoginUseCase);
  private authRepository = inject(AUTH_REPOSITORY);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  loginForm!: FormGroup;
  loading = false;
  submitted = false;
  error = '';
  hidePassword = true;
  returnUrl: string = '/dashboard';

  ngOnInit(): void {
    if (this.authRepository.isAuthenticated()) {
      this.router.navigate(['/dashboard']);
    }

    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      rememberMe: [false]
    });

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/dashboard';
  }

  get f() {
    return this.loginForm.controls;
  }

  onSubmit(): void {
    this.submitted = true;
    this.error = '';

    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;

    this.loginUseCase.execute({
      username: this.f['username'].value,
      password: this.f['password'].value,
      rememberMe: this.f['rememberMe'].value
    }).subscribe({
      next: (result: LoginResult) => {
        if (result.success) {
          this.router.navigate([this.returnUrl]);
        }
      },
      error: () => {
        this.error = 'Invalid username or password';
        this.loading = false;
      }
    });
  }

  togglePasswordVisibility(): void {
    this.hidePassword = !this.hidePassword;
  }
}