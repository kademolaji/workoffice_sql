import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [

  {
    path: 'apptype',
    loadChildren: () =>
      import('./nhs/nhs.module').then((m) => m.NHSModule),
  },

  {
    path: 'hospital',
    loadChildren: () =>
      import('./nhs/nhs.module').then((m) => m.NHSModule),
  },

  {
    path: 'consultant',
    loadChildren: () =>
      import('./nhs/nhs.module').then((m) => m.NHSModule),
  },

  {
    path: 'nhsactivity',
    loadChildren: () =>
      import('./nhs/nhs.module').then((m) => m.NHSModule),
  },

  {
    path: 'pathwaystatus',
    loadChildren: () =>
      import('./nhs/nhs.module').then((m) => m.NHSModule),
  },

  {
    path: 'rtt',
    loadChildren: () =>
      import('./nhs/nhs.module').then((m) => m.NHSModule),
  },

  {
    path: 'specialty',
    loadChildren: () =>
      import('./nhs/nhs.module').then((m) => m.NHSModule),
  },

  {
    path: 'waitingtype',
    loadChildren: () =>
      import('./nhs/nhs.module').then((m) => m.NHSModule),
  },

  {
    path: 'ward',
    loadChildren: () =>
      import('./nhs/nhs.module').then((m) => m.NHSModule),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SetupRoutingModule {}
