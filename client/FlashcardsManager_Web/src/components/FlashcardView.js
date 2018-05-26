import React from 'react';
import * as Ons from 'react-onsenui';

export default class FlashcardView extends React.Component {
    render() {
        const { value, description } = this.props;

        return (
            // <div style={{ padding: '10px 10px 60px 10px', margin: '30px 15px 25px 15px ', border: 'solid 1px', background: '#F0F0F0' }} >
            <Ons.Card style={{ margin: '30px 15px 25px 15px ' }}>
                <h2 className="title" style={{ fontWeight: 'bold' }}>{value}</h2>
                <div className="content">{description}</div>
            </Ons.Card>
            //{/* </div> */}
        );
    }
}