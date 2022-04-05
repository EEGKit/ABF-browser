import React from 'react';

/**
 * Shows ABF file information as a single line
 */
class AbfInfo extends React.Component {

    state = {
        abfInfo: null,
    };

    componentDidMount() {
        const url = `http://192.168.1.9/abf-browser/api/v3/abf-info/?path=` + this.props.abfPath;
        fetch(url)
            .then(response => response.json())
            .then(json => { this.setState({ abfInfo: json }); });
    }

    render() {
        if (this.state.abfInfo == null)
            return <>Loading...</>

        if (this.state.abfInfo.error != null)
            return <mark>ABF API ERROR</mark>

        const protocol = String(this.state.abfInfo.protocol).replace(".pro", "");
        const sweepCount = this.state.abfInfo.sweepCount;
        const sweepLengthSec = this.state.abfInfo.sweepLengthSec;
        const totalLengthSec = sweepLengthSec * this.state.abfInfo.sweepCount;
        const totalLengthMin = Math.round(totalLengthSec / 6) / 10;

        let response = `${protocol}, ${sweepCount} sweeps, ${sweepLengthSec} sec each, ${totalLengthMin} min`;
        if (this.state.abfInfo.tagStrings) {
            response += `, ${this.state.abfInfo.tagStrings}`
        }

        return response;
    }
}

export default AbfInfo;