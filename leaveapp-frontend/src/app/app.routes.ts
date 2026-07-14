import { Routes } from '@angular/router';
import { Login } from './features/login/login'
import { DepartmentComponent } from './features/department/department';
import { authGuard } from './core/guards/auth-guard';
import { PersonnelComponent } from './features/personnel/personnel';
import { LeaveTypeComponent } from './features/leave-type/leave-type';
import { LeaveRequestComponent } from './features/leave-request/leave-request';
import { Auth } from './core/services/auth';

export const routes: Routes = [
    { path: 'login', component: Login},
    { path: 'departman', component: DepartmentComponent, canActivate: [authGuard]},
    { path: '', redirectTo: '/login', pathMatch: 'full'},
    { path: 'personel', component: PersonnelComponent, canActivate: [authGuard] },
    { path: 'izin-turu', component: LeaveTypeComponent, canActivate: [authGuard]},
    { path: 'izin-talep', component: LeaveRequestComponent, canActivate: [authGuard]},
];
