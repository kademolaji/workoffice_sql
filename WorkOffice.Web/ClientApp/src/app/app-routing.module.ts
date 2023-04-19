import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Page404Component } from './authentication/page404/page404.component';
import { AuthGuard } from './core/guard/auth.guard';
import { AuthLayoutComponent } from './layout/app-layout/auth-layout/auth-layout.component';
import { MainLayoutComponent } from './layout/app-layout/main-layout/main-layout.component';
const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: '/account/login', pathMatch: 'full' },
      {
        path: 'admin',
        canActivate: [AuthGuard],
        data: {
          role: ["user roles", "Users", "Organzation"],
        },
        loadChildren: () =>
          import('./admin/admin.module').then((m) => m.AdminModule),
      },
      {
        path: 'nhs',
        canActivate: [AuthGuard],
        data: {
          role: ["user roles", "Users", "Organzation"],
        },
        loadChildren: () =>
          import('./nhs/nhs.module').then((m) => m.NhsModule),
      },
      {
        path: 'setup',
        canActivate: [AuthGuard],
        data: {
          role: ["user roles", ""],
        },
        loadChildren: () =>
          import('./setup/setup.module').then((m) => m.SetupModule),
      },
    ],
  },
  {
    path: 'account',
    component: AuthLayoutComponent,
    loadChildren: () =>
      import('./authentication/authentication.module').then(
        (m) => m.AuthenticationModule
      ),
  },
  { path: '**', component: Page404Component },
];
@NgModule({
  imports: [RouterModule.forRoot(routes, {})],
  exports: [RouterModule],
})
export class AppRoutingModule {}
