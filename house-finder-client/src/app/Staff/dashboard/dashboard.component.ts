import { Component, OnInit } from '@angular/core';
import { HouseService } from 'src/app/services/house.service';
import { RoomTypeService } from 'src/app/services/room-type.service';
import { RoomService } from 'src/app/services/room.service';
import { StatusService } from 'src/app/services/status.service';
import { Chart, registerables } from 'chart.js';
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

  constructor(
    private roomService: RoomService,
    private houseService: HouseService,
    private roomTypeService: RoomTypeService,
    private statusService: StatusService,
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

    //Create chart objects
    var myChart = new Chart("myChart", {
    type: 'bar',
    data: {
        labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
        datasets: [{
            label: '# of Votes',
            data: [12, 19, 3, 5, 2, 3],
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)',
                'rgba(54, 162, 235, 0.2)',
                'rgba(255, 206, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)',
                'rgba(153, 102, 255, 0.2)',
                'rgba(255, 159, 64, 0.2)'
            ],
            borderColor: [
                'rgba(255, 99, 132, 1)',
                'rgba(54, 162, 235, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(75, 192, 192, 1)',
                'rgba(153, 102, 255, 1)',
                'rgba(255, 159, 64, 1)'
            ],
            borderWidth: 1
        }]
    },
    options: {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }
});
  }

}
