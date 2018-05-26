import React from 'react';

import * as Ons from 'react-onsenui';

import NavBar from './NavBar';
import LocationList from '../containers/LocationList';
import AddLocation from '../containers/AddLocation';
import { colors } from '../util/colors';
import Toast from './Toast';

export default class CategoriesListView extends React.Component {
    constructor() {
        super();
        this.state = { openModal: false, name: '' };
    }

    handleAddClick = () => {
        this.props.onAddButtonClick(this.state.name);
        this.setState({ openModal: false });
    }

    handleNameChange = (e) => {
        const name = e.target.value;
        this.setState({ name });
    }

    renderRow = (category, index) => {
        const { onCategorySelect } = this.props;
        const color = colors[index % colors.length];
        if (category == null) {
            return null;
        }
        return (
            <Ons.ListItem key={index} tappable onClick={() => onCategorySelect(category.id)}>
                <div className='left'>
                    <span style={{ background: color, width: '40px', height: '40px', borderRadius: '50%' }} className='list-item_thumbnail' />
                </div>
                <div className='center'>
                    {category.name}
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
                            value={this.state.name}
                            onChange={this.handleNameChange}
                            modifier='material'
                            float
                            placeholder='Nazwa kategorii' />
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
        const { categories, title, onAddButtonClick } = this.props;
        return (
            <Ons.Page renderFixed={this.renderFixed} renderModal={this.renderModal}>
                <Ons.List
                    dataSource={categories}
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