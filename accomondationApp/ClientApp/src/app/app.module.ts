import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { HotelComponent } from './hotel/hotel.component';
import { ReservationformComponent } from './reservationform/reservationform.component';
import { SortPipe } from '../app/pipes/sort.pipe';
import { FilterPipe } from '../app/filter.pipe';
import { SortParamsDirective } from '../app/sorting/sort.params.directive';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,   
    ReservationformComponent,
    HotelComponent,
    FilterPipe,
    SortPipe,
    SortParamsDirective
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,        
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'hotel', component: HotelComponent}
    ])
  ],  
  providers: [],
  bootstrap: [AppComponent, HotelComponent]
})
export class AppModule { }
