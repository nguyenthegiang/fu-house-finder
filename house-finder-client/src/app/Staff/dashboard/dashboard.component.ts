import { Component, OnInit } from '@angular/core';
import { HouseService } from 'src/app/services/house.service';
import { RoomTypeService } from 'src/app/services/room-type.service';
import { RoomService } from 'src/app/services/room.service';
import { StatusService } from 'src/app/services/status.service';
import { Chart, registerables } from 'chart.js';
import { OrderService } from 'src/app/services/order.service';
Chart.register(...registerables);


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardStaffComponent implements OnInit {
  //Total of available rooms
  availabelRoomsNum: number = 0;
  //Total of available capacity
  availableCapNum: number = 0;
  //Total of houses
  totalHouses: number = 0;
  //Total of available houses
  availableHouseNum: number = 0;
  //Array of total orders by month
  orderByMonth: number[] | undefined;
  //Array oof solved orders by month
  solvedOrderByMonth: number[] | undefined;

  constructor(
    private roomService: RoomService,
    private houseService: HouseService,
    private roomTypeService: RoomTypeService,
    private statusService: StatusService,
    private orderService: OrderService,
  ) {
  }

  ngOnInit(): void {
    //Call API: get total of available rooms
    this.roomService.countAvailableRooms().subscribe(data => {
      this.availabelRoomsNum = data;
    });

    //Call API: get total of available capacity
    this.roomService.countAvailableCapacity().subscribe(data => {
      this.availableCapNum = data;
    });

    //Call API: get total houses
    this.houseService.getTotalHouse().subscribe(data => {
      this.totalHouses = data;
    });

    //Call API: get total of available houses
    this.houseService.countTotalAvailableHouse().subscribe(data => {
      this.availableHouseNum = data;
    });

    //Call API: get total orders by month
    this.orderService.getTotalOrderByMonth().subscribe(data => {
      this.orderByMonth = data;

      this.orderService.getSolvedOrderByMonth().subscribe(data => {
        this.solvedOrderByMonth = data;

        //Create chart objects
    var myChart = new Chart("myChart", {

      type: 'line',
      data: {
          labels: ['Tháng 1',
                  'Tháng 2',
                  'Tháng 3',
                  'Tháng 4',
                  'Tháng 5',
                  'Tháng 6',
                  'Tháng 7',
                  'Tháng 8',
                  'Tháng 9',
                  'Tháng 10',
                  'Tháng 11',
                  'Tháng 12',],
          datasets: [
            {
              label: 'Số đơn đăng ký',
              data: this.orderByMonth,
              borderColor: [
                   '#FF6F3D',
              ],
              borderWidth: 1
          },
          {
            label: 'Số đơn đã được giải quyết',
            data: this.solvedOrderByMonth,
            borderColor: [
                '#3E4F3C',
            ],
            borderWidth: 1
        },
        ],

      },
      options: {
          scales: {
              y: {
                  beginAtZero: true
              }
          },
          plugins:{
            title:{
              display: true,
              text: 'Thống kê số lượng đăng ký nhà trọ trong năm',
            }
          }
      }
  });
  //End chart
      })
    });



  }

}
