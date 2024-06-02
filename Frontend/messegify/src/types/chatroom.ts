export interface Chatroom {
    id: string;
    name: string;
    chatRoomType: ChatroomType;
    members: string[];
}

export enum ChatroomType {
    regular = 'Regular',
    direct = 'Direct',
}