import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { SidebarModule } from 'primeng/sidebar';
import { ButtonModule } from 'primeng/button';
import { AppRoutingModule } from '../app-routing.module';


@NgModule({
  declarations: [SidebarComponent],
  imports: [
    CommonModule,
    SidebarModule,
    ButtonModule,
    AppRoutingModule,
  ],
  exports: [
    SidebarComponent
  ]
})
export class SharedModule { }
