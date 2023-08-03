import React, {FC} from 'react';
import {useDisclosure} from "@mantine/hooks";
import {Drawer, Group, ActionIcon} from "@mantine/core";
import {ContactMaker} from "../contactMaker/ContactMaker";
import {IconAdjustments} from '@tabler/icons-react';

interface UtilityDrawerProps {
}

export const UtilityDrawer: FC<UtilityDrawerProps> = ({}) => {
    const [opened, {open, close}] = useDisclosure(false);

    return (
        <>
            <Drawer opened={opened} onClose={close} title="Authentication">
                <ContactMaker/>
            </Drawer>

            <Group position="center">
                <ActionIcon onClick={() => open()} size="xl">
                    <IconAdjustments size="2rem"/>
                </ActionIcon>
            </Group>
        </>
    );
};