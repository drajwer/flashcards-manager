import React from 'react';

import * as Ons from 'react-onsenui';

import NavBar from './NavBar';
import { colors } from '../util/colors';
import Toast from './Toast';

export default class FlashcardsListView extends React.Component {
    constructor() {
        super();
        this.state = { openModal: false, key: '', value: '', keyDesc: '', valueDesc: '' };
    }

    handleAddClick = () => {
        this.props.onAddButtonClick(this.state.key, this.state.value, this.state.keyDesc, this.state.valueDesc);
        this.setState({ openModal: false });
    }

    handleKeyChange = (e) => {
        const key = e.target.value;
        this.setState({ key });
    }

    handleValueChange = (e) => {
        const value = e.target.value;
        this.setState({ value });
    }

    handleKeyDescChange = (e) => {
        const keyDesc = e.target.value;
        this.setState({ keyDesc });
    }

    handleValueDescChange = (e) => {
        const valueDesc = e.target.value;
        this.setState({ valueDesc });
    }

    renderRow = (flashcard, index) => {
        const { onFlashcardSelect } = this.props;
        const color = colors[index % colors.length];
        if (flashcard == null) {
            return null;
        }
        return (
            <Ons.ListItem key={index} tappable onClick={() => onFlashcardSelect(flashcard.id)}>
                <div className='left'>
                    <span style={{ background: color, width: '40px', height: '40px', borderRadius: '50%' }} className='list-item_thumbnail' />
                </div>
                <div className='center'>
                    {flashcard.key}
                </div>
            </Ons.ListItem>
        )
    };
    renderModal = () => {
        if (this.props.onAddButtonClick == null) {
            return null;
        }
        return (
            <Ons.Dialog
                isOpen={this.state.openModal}
                isCancellable
                onCancel={() => this.setState({ openModal: false })}
            >
                <section style={{ margin: '16px' }}>
                    <p>
                        <Ons.Input
                            value={this.state.key}
                            onChange={this.handleKeyChange}
                            modifier='material'
                            float
                            placeholder='Klucz' />
                    </p>
                    <p>
                        <Ons.Input
                            value={this.state.value}
                            onChange={this.handleValueChange}
                            modifier='material'
                            float
                            placeholder='Wartość' />
                    </p>
                    <p>
                        <Ons.Input
                            value={this.state.keyDesc}
                            onChange={this.handleKeyDescChange}
                            modifier='material'
                            float
                            placeholder='Opis klucza' />
                    </p>
                    <p>
                        <Ons.Input
                            value={this.state.valueDesc}
                            onChange={this.handleValueDescChange}
                            modifier='material'
                            float
                            placeholder='Opis wartości' />
                    </p>
                    <div style={{ textAlign: 'center' }}>
                        <Ons.Button style={{ margin: '5px' }} onClick={this.handleAddClick}>
                            Dodaj
                        </Ons.Button>
                        <Ons.Button style={{ margin: '5px' }} onClick={() => this.setState({ openModal: false })}>
                            Anuluj
                        </Ons.Button>
                    </div>
                </section>
            </Ons.Dialog>
        );
    }

    renderFixed = () => {
        const { onAddButtonClick } = this.props;
        if (onAddButtonClick == null) {
            return null;
        }
        return (
            <Ons.Fab position='bottom right' onClick={() => this.setState({ openModal: true })}>+</Ons.Fab>
        );
    }
    render() {
        const { flashcards, title, onAddButtonClick } = this.props;
        return (
            <Ons.Page renderFixed={this.renderFixed} renderModal={this.renderModal}>
                <Ons.List
                    dataSource={flashcards}
                    tappable
                    renderRow={this.renderRow}
                    renderHeader={() => <Ons.ListHeader>{title}</Ons.ListHeader>}
                />

                {onAddButtonClick &&
                    <Toast
                        message={this.props.message}
                        onDismissClick={this.props.onDismissClick}
                        showToast={this.props.showToast}
                    />
                }
            </Ons.Page>
        )
    }
}