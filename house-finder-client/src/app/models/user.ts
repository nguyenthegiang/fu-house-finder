import { Address } from './address';

//DTO: Users
export interface User {
    accessToken: string;
    userId: string;
    facebookUserId: string;
    googleUserId: string;
    email: string;
    password: string;
    displayName: string;
    statusId: number;
    profileImageLink: string;
    phoneNumber: string;
    facebookUrl: string;
    identityCardFrontSideImageLink: string;
    identityCardBackSideImageLink: string;
    roleId: number;
    roleName: string;
    createdDate: Date;
    lastModifiedDate: Date;
    createdBy: string;
    lastModifiedBy: string;

    address: Address;
}
