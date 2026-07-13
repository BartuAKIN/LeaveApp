import { Routes } from '@angular/router';
import { Login } from './features/login/login'
import { DepartmentComponent } from './features/department/department';
import { authGuard } from './core/guards/auth-guard';
import { PersonnelComponent } from './features/personnel/personnel';

export const routes: Routes = [
    { path: 'login', component: Login},
    { path: 'departman', component: DepartmentComponent, canActivate: [authGuard]},
    { path: '', redirectTo: '/login', pathMatch: 'full'},
    { path: 'personel', component: PersonnelComponent, canActivate: [authGuard] },
];
