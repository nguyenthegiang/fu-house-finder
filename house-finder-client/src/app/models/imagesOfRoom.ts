//DTO: ImagesOfRoom
export interface ImagesOfRoom {
  imageId: number;
  imageLink: string;
  roomId: number;
}

export interface ImagesOfRoomUploadData{
  roomName: string;
  building: number;
  floor: number;
  imageIndex: number;
  houseId: number;
}

export interface ImagesOfRoomUploadFileData{
  data: ImagesOfRoomUploadData;
  image: File;
}
