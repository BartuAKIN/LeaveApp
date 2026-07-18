import { managerGuard } from './core/guards/manager-guard';
import { LeaveApproval } from './features/leave-approval/leave-approval';
import { Routes } from '@angular/router';
import { Login } from './features/login/login'
import { DepartmentComponent } from './features/department/department';
import { authGuard } from './core/guards/auth-guard';
import { PersonnelComponent } from './features/personnel/personnel';
import { LeaveTypeComponent } from './features/leave-type/leave-type';
import { LeaveRequestComponent } from './features/leave-request/leave-request';
import { Auth } from './core/services/auth';
import { Dashboard } from './features/dashboard/dashboard';



export const routes: Routes = [
    { path: 'login', component: Login},
    { path: 'departman', component: DepartmentComponent, canActivate: [authGuard, managerGuard]},
    { path: '', redirectTo: '/login', pathMatch: 'full'},
    { path: 'personel', component: PersonnelComponent, canActivate: [authGuard, managerGuard] },
    { path: 'izin-turu', component: LeaveTypeComponent, canActivate: [authGuard, managerGuard]},
    { path: 'izin-talep', component: LeaveRequestComponent, canActivate: [authGuard]},
    { path: 'izin-onay', component: LeaveApproval, canActivate: [authGuard, managerGuard] },
    { path: 'dashboard', component: Dashboard, canActivate: [authGuard] },
];
