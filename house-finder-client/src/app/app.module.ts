import { SafePipe } from './pipe/safePipe';
import { HttpClientModule } from '@angular/common/http';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule, routingComponents } from './app-routing.module';
import { AppComponent } from './app.component';
import { HouseDetailComponent } from './Guest/house-detail/house-detail.component';
import { LoginComponent } from './Guest/login/login.component';
import { DashboardComponent } from './Landlord/dashboard/dashboard.component';
import { ListReportComponent } from './Staff/list-report/list-report.component';
import { ListHouseComponent } from './Staff/list-house/list-house.component';
import { DashboardStaffComponent } from './Staff/dashboard/dashboard.component';
import { UpdateRoomComponent } from './Landlord/update-room/update-room.component';
import { ListLandlordComponent } from './Staff/list-landlord/list-landlord.component';
import { ListLandlordSignupRequestComponent } from './Staff/list-landlord-signup-request/list-landlord-signup-request.component';
import { RoomDetailComponent } from './Guest/room-detail/room-detail.component';
import { AddHouseComponent } from './Landlord/add-house/add-house.component';
import { UpdateHouseComponent } from './Landlord/update-house/update-house.component';
import { LandlordHouseDetailComponent } from './Landlord/landlord-house-detail/landlord-house-detail.component';
import { RateHouseComponent } from './Landlord/rate-house/rate-house.component';
import { ListOrderComponent } from './Staff/list-order/list-order.component';
import { AddRoomComponent } from './Landlord/add-room/add-room.component';
import { SingleComponent } from './Landlord/add-room/single/single.component';
import { MultipleComponent } from './Landlord/add-room/multiple/multiple.component';
import { MaterialModule } from './materials/material.module';
import { ListStaffComponent } from './Admin/list-staff/list-staff.component';
import { CreateAccountComponent } from './Admin/create-account/create-account.component';
import { UpdateAccountComponent } from './Admin/update-account/update-account.component';
import { HeaderComponent } from './Common/header/header.component';
import { FooterComponent } from './Common/footer/footer.component';
import { RoleModalComponent } from './Guest/login/role-modal/role-modal.component';
import { StaffHeaderComponent } from './Common/staff-header/staff-header.component';
import { StaffStatisticsComponent } from './Common/staff-statistics/staff-statistics.component';
import { StaffLandlordDetailComponent } from './Staff/staff-landlord-detail/staff-landlord-detail.component';
import { StaffHouseDetailComponent } from './Staff/staff-house-detail/staff-house-detail.component';
import { StaffRoomDetailComponent } from './Staff/staff-room-detail/staff-room-detail.component';
import { DeleteHouseComponent } from './Landlord/delete-house/delete-house.component';
import { LandlordDetailInfoComponent } from './Staff/list-landlord/landlord-detail-info/landlord-detail-info.component';
import { StaffLandlordDetailInfoComponent } from './Staff/staff-landlord-detail/staff-landlord-detail-info/staff-landlord-detail-info.component';
import { DashboardInfoComponent } from './Landlord/dashboard/dashboard-info/dashboard-info.component';
import { ListHouseInfoComponent } from './Staff/list-house/list-house-info/list-house-info.component';
import { CreateOrderComponent } from './Guest/create-order/create-order.component';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { LandlordHeaderComponent } from './Common/landlord-header/landlord-header.component';
import { AdminHeaderComponent } from './Common/admin-header/admin-header.component';
import { StaffUpdateProfileComponent } from './Staff/staff-update-profile/staff-update-profile.component';
import { LandlordUpdateProfileComponent } from './Landlord/landlord-update-profile/landlord-update-profile.component';
import { StaffChangePasswordComponent } from './Staff/staff-change-password/staff-change-password.component';
import { LandlordChangePasswordComponent } from './Landlord/landlord-change-password/landlord-change-password.component';

@NgModule({
  declarations: [
    AppComponent,
    routingComponents,
    SafePipe,
    HouseDetailComponent,
    LoginComponent,
    DashboardComponent,
    DashboardStaffComponent,
    ListReportComponent,
    ListHouseComponent,
    UpdateRoomComponent,
    ListLandlordComponent,
    ListLandlordSignupRequestComponent,
    RoomDetailComponent,
    AddHouseComponent,
    UpdateHouseComponent,
    LandlordHouseDetailComponent,
    RateHouseComponent,
    ListOrderComponent,
    AddRoomComponent,
    SingleComponent,
    MultipleComponent,
    ListStaffComponent,
    CreateAccountComponent,
    UpdateAccountComponent,
    HeaderComponent,
    FooterComponent,
    RoleModalComponent,
    StaffHeaderComponent,
    StaffStatisticsComponent,
    StaffLandlordDetailComponent,
    StaffHouseDetailComponent,
    StaffRoomDetailComponent,
    DeleteHouseComponent,
    LandlordDetailInfoComponent,
    StaffLandlordDetailInfoComponent,
    DashboardInfoComponent,
    ListHouseInfoComponent,
    CreateOrderComponent,
    LandlordHeaderComponent,
    AdminHeaderComponent,
    StaffUpdateProfileComponent,
    LandlordUpdateProfileComponent,
    StaffChangePasswordComponent,
    LandlordChangePasswordComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    BrowserAnimationsModule,
    SweetAlert2Module.forRoot(),
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
