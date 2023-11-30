import React, {FC} from 'react';
import {useDisclosure} from "@mantine/hooks";
import {Drawer, Group, ActionIcon, Flex, Title} from "@mantine/core";
import {ContactMakerButton} from "./ContactMakerButton";
import {IconAdjustments} from '@tabler/icons-react';
import {DeleteAccountButton} from "./DeleteAccountButton";
import {UpdateAccountButton} from "./UpdateAccountButton";
import {DeleteContactButton} from "./DeleteContactButton";

interface UtilityDrawerProps {
}

export const UtilityDrawer: FC<UtilityDrawerProps> = () => {
    const [opened, {open, close}] = useDisclosure(false);

    return (
        <>
            <Drawer opened={opened} onClose={close}>
                <Title order={3} weight={100} align="center">
                    Account Utilities
                </Title>

                <Flex
                    mih={300}
                    gap="xl"
                    justify="center"
                    align="center"
                    direction="column"
                    wrap="wrap"
                >
                    <ContactMakerButton/>
                    <UpdateAccountButton/>
                    <DeleteContactButton/>
                    <DeleteAccountButton/>
                </Flex>
            </Drawer>

            <Group position="center">
                <ActionIcon onClick={() => open()} size="xl">
                    <IconAdjustments size="2rem"/>
                </ActionIcon>
            </Group>
        </>
    );
};