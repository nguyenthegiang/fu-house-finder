//DTO: ImagesOfRoom
export interface ImagesOfRoom {
  imageId: number;
  imageLink: string;
  roomId: number;
}

export interface ImagesOfRoomUploadData{
  roomName: string;
  buildingNumber: number;
  floorNumber: number;
  houseId: number;
}

export interface ImagesOfRoomUploadFileData{
  data: ImagesOfRoomUploadData;
  image: File;
}
