import { RoomCapacity } from "../interfaces/RoomCapacity"


export interface Room{
  roomId: number,
  roomNumber: string,
  roomCapacityId: number,
  roomCapacity: RoomCapacity,
}
