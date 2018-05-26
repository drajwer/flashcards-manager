import React from 'react';

import * as Ons from 'react-onsenui';

export default class SelectModeView extends React.Component {     
    renderRow = (mode, index) => {
        return (
            <Ons.ListItem key={index} tappable onClick={() => this.props.onModeSelect(mode.modeEnum)}>
                <div className='left'>
                    <span style={{ background: mode.color, width: '40px', height: '40px', borderRadius: '50%' }} className='list-item_thumbnail' />
                </div>
                <div className="list-item__center list-item--material__center">
                    <div style={{ fontWeight: 'bold'}} className="list-item__title list-item--material__title">{mode.name}</div>
                    <div style={{}} className="list-item__subtitle list-item--material__subtitle">{mode.description}</div>
                </div>
            </Ons.ListItem>
        )
    }
    render() {
        const modes = [
            {
                name: 'Tylko nowe',
                description: 'Pokaż fiszki, których jeszcze nie znam.',
                color: '#ffe119',
                modeEnum: 1
            },
            {
                name: 'Tylko znane',
                description: 'Pokaż fiszki, które już widziałem.',
                color: '#000080',
                modeEnum: 2
            },
            {
                name: 'Tylko trudne',
                description: 'Pokaż fiszki, które sprawiły mi problem.',
                color: '#e6194b',
                modeEnum: 3
            },
            {
                name: 'Tylko już opanowane',
                description: 'Pokaż fiszki, które dobrze znam.',
                color: '#3cb44b',
                modeEnum: 4
            },
            {
                name: 'Wszystkie',
                description: 'Pokaż mi fiszki losowo.',
                color: '#911eb4',
                modeEnum: 0
            },
        ];
        return (
            <div>
                <Ons.List
                    dataSource={modes}
                    renderRow={this.renderRow}
                />
            </div>
        )
    }
}