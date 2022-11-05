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
  //Array total of orders by month
  orderByMonth: number[] | undefined;

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
    });
    console.log("orders by month:" + this.orderByMonth);

    this.orderByMonth = [12, 13, 14, 10, 9, 9, 0, 10, 12];

    //Create chart objects
    var myChart = new Chart("myChart", {

    type: 'bar',
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
        datasets: [{
            label: 'Số đơn đăng ký',
            data: this.orderByMonth,
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)',
                'rgba(54, 162, 235, 0.2)',
                'rgba(255, 206, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)',
                'rgba(153, 102, 255, 0.2)',
                'rgba(255, 159, 64, 0.2)',

                // 'rgba(255, 99, 132, 0.2)',
                // 'rgba(54, 162, 235, 0.2)',
                // 'rgba(255, 206, 86, 0.2)',
                // 'rgba(75, 192, 192, 0.2)',
                // 'rgba(153, 102, 255, 0.2)',
                // 'rgba(255, 159, 64, 0.2)',

                // 'rgba(255, 99, 132, 0.2)',
                // 'rgba(54, 162, 235, 0.2)',
            ],
            borderColor: [
                'rgba(255, 99, 132, 1)',
                'rgba(54, 162, 235, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(75, 192, 192, 1)',
                'rgba(153, 102, 255, 1)',
                'rgba(255, 159, 64, 1)',

                // 'rgba(255, 99, 132, 1)',
                // 'rgba(54, 162, 235, 1)',
                // 'rgba(255, 206, 86, 1)',
                // 'rgba(75, 192, 192, 1)',
                // 'rgba(153, 102, 255, 1)',
                // 'rgba(255, 159, 64, 1)',

                // 'rgba(255, 99, 132, 1)',
                // 'rgba(54, 162, 235, 1)',
            ],
            borderWidth: 1
        }]
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
            text: 'Thống kê số lượng đăng ký nhà trọ',
          }
        }
    }
});
  }

}
