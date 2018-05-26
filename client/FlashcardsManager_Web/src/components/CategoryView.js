import React from 'react';

import * as Ons from 'react-onsenui';
import ons from 'onsenui';

import { colors } from '../util/colors';
import { categoryAvailability } from '../api/types';
import Toast from './Toast';


export default class CategoriesListView extends React.Component {
    constructor() {
        super();
        this.state = { openModal: false, name: '', showActionSheet: false };
    }
    handleNameChange = (e) => {
        const name = e.target.value;
        this.setState({ name });
    }
    handleSaveNameClick = () => {
        this.props.onUpdateName(this.state.name);
        this.setState({ openModal: false, name: '' });
    }
    renderFixed = () => {
        const { category } = this.props;
        if (category.availability !== categoryAvailability['Private']) {
            return (
                <Ons.Fab position='bottom right' onClick={this.handleDeleteClick}>
                    <Ons.Icon icon="md-delete" />
                </Ons.Fab>
            );
        }
        return (
            <div>
                <Ons.Fab position='bottom right' onClick={() => this.setState({ showActionSheet: true })}>
                    <Ons.Icon icon="md-settings" />
                </Ons.Fab>
            </div>
        );
    }
    handleNameChangeClick = () => {
        const category = this.props.category;
        this.setState({ name: category.name, openModal: true });
    }
    handleActionSheetCancel = () => {
        this.setState({ showActionSheet: false });
    }

    handleDeleteClick = () => {
        ons.notification.confirm({
            message: 'Czy na pewno chcesz usunąć tę kategorię?',
            title: 'Potwierdź',
            buttonLabels: ['Nie', 'Tak'],
            callback: index => {
                if (index !== 1)
                    return;
                this.props.onDeleteClick();
            }
        });
        this.handleActionSheetCancel();
    }

    handlePublicClick = () => {
        ons.notification.confirm({
            message: 'Czy na pewno chcesz opublikować tę kategorię?',
            title: 'Potwierdź',
            buttonLabels: ['Nie', 'Tak'],
            callback: index => {
                if (index !== 1)
                    return;
                this.props.onPublicClick();
            }
        });
        this.handleActionSheetCancel();
    }

    renderModal = () => {
        return (
            <Ons.Dialog
                isOpen={this.state.openModal}
                isCancellable
                onCancel={() => this.setState({ openModal: false })}
            >
                <section style={{ margin: '16px' }}>
                    <p>
                        <Ons.Input
                            value={this.state.name}
                            onChange={this.handleNameChange}
                            modifier='material'
                            float
                            placeholder='Nazwa kategorii' />
                    </p>
                    <div style={{ textAlign: 'center' }}>
                        <Ons.Button style={{ margin: '5px' }} onClick={this.handleSaveNameClick}>
                            Zapisz
                        </Ons.Button>
                        <Ons.Button style={{ margin: '5px' }} onClick={() => this.setState({ openModal: false })}>
                            Anuluj
                        </Ons.Button>
                    </div>
                </section>
            </Ons.Dialog>
        );
    }

    render() {
        const { showActionSheet } = this.state;
        const { category, onFlashcardsClick } = this.props;
        const labelStyle = { fontWeight: 'bold' };
        return (
            <Ons.Page renderFixed={this.renderFixed} renderModal={this.renderModal}>
                <div>
                    <Ons.List tappable>
                        <Ons.ListItem onClick={this.handleNameChangeClick}>
                            <div style={labelStyle} className='center'>
                                Nazwa
                        </div>
                            <div className='right' style={{ textOverflow: 'ellipsis', wordBreak: 'break-all' }}>
                                {category.name}
                            </div>
                        </Ons.ListItem>
                        <Ons.ListItem>
                            <div style={labelStyle} className='center'>
                                Typ
                        </div>
                            <div className='right'>
                                <div>{this.getCategoryAvailabilty(category)}</div>
                            </div>

                        </Ons.ListItem>
                        <Ons.ListItem onClick={onFlashcardsClick}>
                            <div style={labelStyle} className='center'>
                                Fiszki
                        </div>
                            <div className='right'>
                                <Ons.Icon icon='arrow-right' />
                            </div>
                        </Ons.ListItem>
                    </Ons.List>

                    <Ons.ActionSheet isOpen={showActionSheet}
                        onCancel={this.handleActionSheetCancel}
                        isCancelable
                        title={'Co chcesz zrobić?'}
                    >
                        <Ons.ActionSheetButton onClick={this.handlePublicClick} icon={'fa-users'}>Opublikuj</Ons.ActionSheetButton>
                        <Ons.ActionSheetButton onClick={this.handleDeleteClick} icon={'md-delete'}>Usuń</Ons.ActionSheetButton>
                        <Ons.ActionSheetButton onClick={this.handleActionSheetCancel} icon={'md-close'}>Powrót</Ons.ActionSheetButton>
                    </Ons.ActionSheet>

                    <Toast
                        message={this.props.message}
                        onDismissClick={this.props.onDismissClick}
                        showToast={this.props.showToast}
                    />
                </div>
            </Ons.Page>
        );
    }
    getCategoryAvailabilty(category) {
        switch (category.availability) {
            case categoryAvailability['Private']:
                return "Prywatna";
            case categoryAvailability['Pending']:
                return "Oczekująca";
            case categoryAvailability['Public']:
                return "Publiczna";
        }
        return "Nieznana";
    }
}