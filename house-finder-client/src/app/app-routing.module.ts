import { HomepageComponent } from './homepage/homepage.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HouseDetailComponent} from './house-detail/house-detail.component';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './Landlord/dashboard/dashboard.component';
import { ListRoomComponent } from './Landlord/list-room/list-room.component';
import { UploadHouseInfoSingleComponent } from './upload-house-info-single/upload-house-info-single.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomepageComponent },
  { path: 'login', component: LoginComponent},
  { path: 'house-detail/:id', component: HouseDetailComponent},
  { path: 'house-detail', component: HouseDetailComponent},
  { path: 'Landlord/dashboard', component: DashboardComponent},
  { path: 'Landlord/list-room', component: ListRoomComponent},
  { path: 'Landlord/upload/single', component: UploadHouseInfoSingleComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})

export class AppRoutingModule {}
export const routingComponents = [HomepageComponent]
