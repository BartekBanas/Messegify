import React, {FC} from 'react';

interface ContentProps {
    children: React.ReactNode;
}

export const Content: FC<ContentProps> = ({children}) => {
    return (
        <div>
            {children}
        </div>
    );
};