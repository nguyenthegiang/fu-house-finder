import { Component, OnInit } from '@angular/core';
import { HouseService } from 'src/app/services/house.service';
import { RoomTypeService } from 'src/app/services/room-type.service';
import { RoomService } from 'src/app/services/room.service';
import StatusService from 'src/app/services/roomStatus.service';
import { Chart, registerables } from 'chart.js';
import { OrderService } from 'src/app/services/order.service';
import { ReportService } from 'src/app/services/report.service';
import { UserService } from 'src/app/services/user.service';
import { CommuneService } from 'src/app/services/commune.service';
import { DistrictService } from 'src/app/services/district.service';
import { VillageService } from 'src/app/services/village.service';
Chart.register(...registerables);

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardStaffComponent implements OnInit {
  //Total of available rooms
  availabelRoomsNum: number = 0;
  //Total of available capacity
  availableCapNum: number = 0;
  //Total of houses
  totalHouses: number = 0;
  //Total of rooms
  totalRooms: number = 0;
  //Total of landlords
  totalLandlords: number = 0;
  activeLandlordNum: number = 0;
  inactiveLandlordNum: number = 0;
  //Total of available houses
  availableHouseNum: number = 0;
  //Array of total orders by month
  orderByMonth: number[] | undefined;
  //Array oof solved orders by month
  solvedOrderByMonth: number[] | undefined;
  //Array of total orders by month
  reportByMonth: number[] | undefined;

  //Burn up chart
  orderByMonthForBurnUp: number[] | undefined;
  orderSolvedByMonthForBurnUp: number[] | undefined;

  //Get current year
  currentYear: number = new Date().getFullYear();

  //Statistics
  totalCapacity: number = 0;
  totallyAvailableCapacity: number = 0;
  partiallyAvailableCapacity: number = 0;

  //Number of villages, communes, districts having houses
  totalVillage: number = 0;
  totalCommune: number = 0;
  totalDistrict: number = 0;

  constructor(
    private roomService: RoomService,
    private houseService: HouseService,
    private reportService: ReportService,
    private userService: UserService,
    private orderService: OrderService,
    private villageService: VillageService,
    private communeService: CommuneService,
    private districtService: DistrictService
  ) { }

  ngOnInit(): void {
    /**
     * [Authorization]
     * Role: Staff
     */
    var userRole = localStorage.getItem("role");
    if (userRole == null || userRole.indexOf('Department') < 0) {
      window.location.href = '/home';
    }

    //Call API:
    this.userService.countTotalLandlords().subscribe((data) => {
      this.totalLandlords = data;

      this.userService.countActiveLandlords().subscribe((data) => {
        this.activeLandlordNum = data;

        this.userService.countInactiveLandlords().subscribe((data) => {
          this.inactiveLandlordNum = data;

          var landlordChart = new Chart('landlordChart', {
            type: 'pie',
            data: {
              labels: ['Hoạt động', 'Không hoạt động', 'Yêu cầu'],
              datasets: [
                {
                  data: [
                    this.activeLandlordNum,
                    this.inactiveLandlordNum,
                    this.totalLandlords -
                    this.activeLandlordNum -
                    this.inactiveLandlordNum,
                  ],
                  backgroundColor: ['#ff9020', '#2fcaca', '#ff6384'],
                },
              ],
            },
            options: {
              plugins: {
                title: {
                  display: true,
                  text: 'Thống kê số chủ trọ',
                  font: {
                    size: 15,
                  },
                },
              },
            },
          });
        });
      });
    });

    //Call API: get number of total rooms
    this.roomService.CountTotalRoom().subscribe((data) => {
      this.totalRooms = data;

      //Call API: get total of available rooms
      this.roomService.countAvailableRooms().subscribe((data) => {
        this.availabelRoomsNum = data;

        var roomChart = new Chart('roomChart', {
          type: 'pie',
          data: {
            labels: ['Hết chỗ', 'Còn trống'],
            datasets: [
              {
                data: [
                  this.totalRooms - this.availabelRoomsNum,
                  this.availabelRoomsNum,
                ],
                backgroundColor: ['#ff9020', '#2fcaca'],
              },
            ],
          },
          options: {
            plugins: {
              title: {
                display: true,
                text: 'Thống kê số phòng trọ',
                font: {
                  size: 15,
                },
              },
            },
          },
        });
      });
    });

    //Call API: get total of available capacity
    this.roomService.countAvailableCapacity().subscribe((data) => {
      this.availableCapNum = data;
    });

    //Call API: get total houses
    this.houseService.getTotalHouse().subscribe((data) => {
      this.totalHouses = data;

      //Call API: get total of available houses
      this.houseService.countTotalAvailableHouse().subscribe((data) => {
        this.availableHouseNum = data;

        console.log(
          'total: ' + this.totalHouses + ' avai: ' + this.availableHouseNum
        );

        //Create house chart
        var houseChart = new Chart('houseChart', {
          type: 'pie',
          data: {
            labels: ['Hết chỗ', 'Còn trống'],
            datasets: [
              {
                data: [
                  this.totalHouses - this.availableHouseNum,
                  this.availableHouseNum,
                ],
                backgroundColor: ['#ff4970', '#069bff'],
              },
            ],
          },
          options: {
            plugins: {
              title: {
                display: true,
                text: 'Thống kê số nhà trọ',
                font: {
                  size: 15,
                },
              },
            },
          },
        });
      });
    });

    //Call API: get total orders by month
    this.orderService.getTotalOrderByMonth().subscribe((data) => {
      this.orderByMonth = data;

      this.orderService.getSolvedOrderByMonth().subscribe((data) => {
        this.solvedOrderByMonth = data;

        //Create order chart
        var orderChart = new Chart('orderChart', {
          type: 'bar',
          data: {
            labels: [
              'Tháng 1',
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
              'Tháng 12',
            ],
            datasets: [
              {
                label: 'Số nguyện vọng đăng ký',
                data: this.orderByMonth,
                backgroundColor: ['#ff6384'],
                borderColor: ['#ff6384'],
                borderWidth: 1,
              },
              {
                label: 'Số nguyện vọng đã được giải quyết',
                data: this.solvedOrderByMonth,
                backgroundColor: ['#069bff'],
                borderColor: ['#069bff'],
                borderWidth: 1,
              },
            ],
          },
          options: {
            scales: {
              y: {
                beginAtZero: true,
              },
            },
            plugins: {
              title: {
                display: true,
                text:
                  'Thống kê số lượng nguyện vọng đăng ký nhà trọ từng tháng trong năm ' + this.currentYear,
                font: {
                  size: 15,
                },
              },
            },
          },
        });

        //End chart
      });
    });

    //Call API: get total reports by month in the current year
    this.reportService.getTotalReportByMonth().subscribe((data) => {
      this.reportByMonth = data;
      //Create report chart
      var reportChart = new Chart('reportChart', {
        type: 'line',
        data: {
          labels: [
            'Tháng 1',
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
            'Tháng 12',
          ],
          datasets: [
            {
              label: 'Số báo cáo',
              data: this.reportByMonth,
              backgroundColor: ['#ff6384'],
              borderColor: ['#ff6384'],
              borderWidth: 1,
            },
          ],
        },
        options: {
          scales: {
            y: {
              beginAtZero: true,
            },
          },
          plugins: {
            title: {
              display: true,
              text: 'Thống kê số lượng báo cáo nhà trọ năm ' + this.currentYear,
              font: {
                size: 15,
              },
            },
          },
        },
      });
    });

    //Call API: Creat capacity chart
    this.roomService.CountTotalCapacity().subscribe((data) => {
      this.totalCapacity = data;

      this.roomService.CountTotallyAvailableRoom().subscribe((data) => {
        this.totallyAvailableCapacity = data;
        this.partiallyAvailableCapacity =
          this.availableCapNum - this.totallyAvailableCapacity;

        //Create chart
        var capacityChart = new Chart('capacityChart', {
          type: 'pie',
          data: {
            labels: ['Hết chỗ', 'Hoàn toàn trống', 'Một phần trống'],
            datasets: [
              {
                data: [
                  this.totalCapacity - this.availableCapNum,
                  this.totallyAvailableCapacity,
                  this.partiallyAvailableCapacity,
                ],
                backgroundColor: ['#ed4756', '#1d6e9d', '#ff9f40'],
              },
            ],
          },
          options: {
            plugins: {
              title: {
                display: true,
                text: 'Thống kê sức chứa',
                font: {
                  size: 15,
                },
              },
            },
          },
        });
      });
    });

    //Call API: Count number of villages, communes, districts having house
    this.villageService.countVillageHavingHouse().subscribe((data) => {
      this.totalVillage = data;
      console.log(this.totalVillage);
    });

    this.communeService.countCommuneHavingHouse().subscribe((data) => {
      this.totalCommune = data;
      console.log(this.totalCommune);
    });

    this.districtService.countDistrictHavingHouse().subscribe((data) => {
      this.totalDistrict = data;
      console.log(this.totalDistrict);
    });

    //Call API to creat burn up chart
    this.orderService.countTotalOrderByMonth().subscribe((data) => {
      this.orderByMonthForBurnUp = data;

      this.orderService.countSolvedOrderByMonth().subscribe((data) => {
        this.orderSolvedByMonthForBurnUp = data;

        //Create burn up chart for order
        var orderBurnUpChart = new Chart('orderBurnUpChart', {
          type: 'line',
          data: {
            labels: [
              'Tháng 1',
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
              'Tháng 12',
            ],
            datasets: [
              {
                label: 'Tổng số nguyện vọng đăng ký',
                data: this.orderByMonthForBurnUp,
                backgroundColor: ['#ff6384'],
                borderColor: ['#ff6384'],
                borderWidth: 1,
              },
              {
                label: 'Tổng số nguyện vọng đã giải quyết',
                data: this.orderSolvedByMonthForBurnUp,
                backgroundColor: ['#069bff'],
                borderColor: ['#069bff'],
                borderWidth: 1,
              },
            ],
          },
          options: {
            scales: {
              y: {
                beginAtZero: true,
              },
            },
            plugins: {
              title: {
                display: true,
                text: 'Thống kê tổng số lượng nguyện vọng đăng ký nhà trọ tới từng tháng trong năm ' + this.currentYear,
                font: {
                  size: 15,
                },
              },
            },
          },
        });
      })
    })
  }
}
