import { Address } from './address';

//DTO: Users
export interface User {
    userId: string;
    facebookUserId: string;
    googleUserId: string;
    email: string;
    displayName: string;
    active: boolean;
    profileImageLink: string;
    phoneNumber: string;
    facebookUrl: string;
    identityCardFrontSideImageLink: string;
    identityCardBackSideImageLink: string;
    roleId: number;
    createdDate: Date;
    lastModifiedDate: Date;
    createdBy: string;
    lastModifiedBy: string;

    address: Address;
}
